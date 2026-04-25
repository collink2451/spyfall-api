using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;
using SpyFall.Api.DTOs;
using SpyFall.Api.Models;

namespace SpyFall.Api.Hubs;

public class GameHub(AppDbContext db) : Hub
{
	private readonly AppDbContext mDb = db;

	// In-memory vote tracking: gameCode -> (accusedPlayerId, votes: playerId -> guilty)
	private static readonly Dictionary<string, (int AccusedId, Dictionary<int, bool> Votes)> ActiveVotes = [];

	public async Task JoinRoom(string code, int playerId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, code);

		Player? player = await mDb.Players.FindAsync(playerId);
		if (player == null) return;

		player.ConnectionId = Context.ConnectionId;

		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Id == player.GameId);

		if (game == null) return;

		// First player to connect becomes the host
		game.HostPlayerId ??= playerId;

		await mDb.SaveChangesAsync();

		List<PlayerResponse> players = await mDb.Players
			.Where(p => p.GameId == player.GameId)
			.Select(p => new PlayerResponse { Id = p.Id, Name = p.Name, IsReady = p.IsReady })
			.ToListAsync();

		await Clients.Group(code).SendAsync("PlayerJoined", players);
	}

	public async Task StartGame(string code, int requestingPlayerId)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return;
		if (game.HostPlayerId != requestingPlayerId) return;
		if (game.Players.Count < 3)
		{
			await Clients.Caller.SendAsync("Error", "At least 3 players are required to start.");
			return;
		}
		if (game.Players.Any(p => !p.IsReady))
		{
			await Clients.Caller.SendAsync("Error", "All players must be ready before starting.");
			return;
		}

		Location location = await mDb.Locations
			.Include(x => x.Roles)
			.OrderBy(x => Guid.NewGuid())
			.FirstAsync();

		game.LocationId = location.Id;

		Player spy = game.Players
			.OrderBy(x => Guid.NewGuid())
			.First();

		spy.IsSpy = true;

		foreach (Player player in game.Players)
		{
			if (player.IsSpy) continue;
			player.RoleId = location.Roles.OrderBy(x => Guid.NewGuid()).First().Id;
		}

		game.LastActivityAt = DateTime.UtcNow;
		game.Status = GameStatus.InProgress;

		await mDb.SaveChangesAsync();

		foreach (Player player in game.Players)
		{
			if (player.IsSpy)
			{
				await Clients.Client(player.ConnectionId).SendAsync("GameStarted", null, null);
			}
			else
			{
				Role role = location.Roles.First(r => r.Id == player.RoleId);
				await Clients.Client(player.ConnectionId).SendAsync("GameStarted", role.Name, location.Name);
			}
		}
	}

	public async Task SetReady(string code, int playerId, bool isReady)
	{
		Player? player = await mDb.Players.FindAsync(playerId);
		if (player == null) return;

		player.IsReady = isReady;
		await mDb.SaveChangesAsync();

		List<PlayerResponse> players = await mDb.Players
			.Where(p => p.GameId == player.GameId)
			.Select(p => new PlayerResponse { Id = p.Id, Name = p.Name, IsReady = p.IsReady })
			.ToListAsync();

		await Clients.Group(code).SendAsync("ReadyStateChanged", players);
	}

	public async Task LeaveGame(string code, int playerId)
	{
		await RemovePlayerInternal(code, playerId);
	}

	public async Task KickPlayer(string code, int hostPlayerId, int targetPlayerId)
	{
		Game? game = await mDb.Games
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return;
		if (game.HostPlayerId != hostPlayerId) return;

		await RemovePlayerInternal(code, targetPlayerId, kicked: true);
	}

	private async Task RemovePlayerInternal(string code, int playerId, bool kicked = false)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return;

		Player? player = game.Players.FirstOrDefault(p => p.Id == playerId);
		if (player == null) return;

		// Notify the removed player
		if (!string.IsNullOrEmpty(player.ConnectionId))
		{
			await Clients.Client(player.ConnectionId).SendAsync("RemovedFromGame", kicked ? "You were kicked." : "You left the game.");
			await Groups.RemoveFromGroupAsync(player.ConnectionId, code);
		}

		game.Players.Remove(player);
		mDb.Players.Remove(player);

		// If no players left, delete the game
		if (game.Players.Count == 0)
		{
			mDb.Games.Remove(game);
			await mDb.SaveChangesAsync();
			return;
		}

		// If the leaving player was the host, assign host to the next player
		if (game.HostPlayerId == playerId)
		{
			game.HostPlayerId = game.Players.First().Id;
			await Clients.Client(game.Players.First().ConnectionId).SendAsync("PromotedToHost");
		}

		game.LastActivityAt = DateTime.UtcNow;
		await mDb.SaveChangesAsync();

		List<PlayerResponse> players = game.Players
			.Select(p => new PlayerResponse { Id = p.Id, Name = p.Name, IsReady = p.IsReady })
			.ToList();
		await Clients.Group(code).SendAsync("PlayerLeft", players);
	}

	public async Task AccusePlayer(string code, int accusedPlayerId)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null || game.Status != GameStatus.InProgress) return;

		ActiveVotes[code] = (accusedPlayerId, []);

		Player? accusedPlayer = game.Players.FirstOrDefault(p => p.Id == accusedPlayerId);
		if (accusedPlayer == null) return;

		await Clients.Group(code).SendAsync("AccusationStarted", accusedPlayer.Name);
	}

	public async Task CastVote(string code, int votingPlayerId, bool guilty)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null || !ActiveVotes.TryGetValue(code, out (int AccusedId, Dictionary<int, bool> Votes) voteState)) return;

		voteState.Votes[votingPlayerId] = guilty;

		int totalPlayers = game.Players.Count;
		int majority = totalPlayers / 2 + 1;
		int guiltyVotes = voteState.Votes.Values.Count(v => v);
		int notGuiltyVotes = voteState.Votes.Values.Count(v => !v);

		if (guiltyVotes >= majority)
		{
			ActiveVotes.Remove(code);
			Player accused = game.Players.First(p => p.Id == voteState.AccusedId);
			bool spyCaught = accused.IsSpy;
			await EndGameInternal(code, game, spyCaught ? "PlayersWin" : "SpyWins");
		}
		else if (notGuiltyVotes >= majority)
		{
			ActiveVotes.Remove(code);
			await Clients.Group(code).SendAsync("VoteResult", "NotGuilty");
		}
		else
		{
			await Clients.Group(code).SendAsync("VoteTally", guiltyVotes, notGuiltyVotes, totalPlayers);
		}
	}

	public async Task SpyGuessLocation(string code, string locationGuess)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.Include(x => x.Location)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null || game.Status != GameStatus.InProgress || game.Location == null) return;

		bool correct = string.Equals(game.Location.Name, locationGuess, StringComparison.OrdinalIgnoreCase);
		await EndGameInternal(code, game, correct ? "SpyWins" : "PlayersWin");
	}

	public async Task EndGame(string code, int requestingPlayerId, bool spyWon)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.Include(x => x.Location)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return;
		if (game.HostPlayerId != requestingPlayerId) return;

		await EndGameInternal(code, game, spyWon ? "SpyWins" : "PlayersWin");
	}

	private async Task EndGameInternal(string code, Game game, string outcome)
	{
		Player? spy = game.Players.FirstOrDefault(p => p.IsSpy);
		string spyName = spy?.Name ?? "Unknown";
		string locationName = game.Location?.Name ?? "Unknown";

		await Clients.Group(code).SendAsync("GameEnded", outcome, locationName, spyName);

		game.Status = GameStatus.Waiting;
		game.LocationId = null;
		game.LastActivityAt = DateTime.UtcNow;

		foreach (Player player in game.Players)
		{
			player.IsSpy = false;
			player.RoleId = null;
			player.IsReady = false;
		}

		await mDb.SaveChangesAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		Player? player = await mDb.Players
			.Include(x => x.Game)
			.FirstOrDefaultAsync(p => p.ConnectionId == Context.ConnectionId);

		if (player != null)
		{
			if (player.Game.Status != GameStatus.InProgress)
			{
				await RemovePlayerInternal(player.Game.Code, player.Id);
			}
		}

		await base.OnDisconnectedAsync(exception);
	}
}
