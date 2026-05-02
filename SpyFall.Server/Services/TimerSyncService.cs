using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SpyFall.Server.Data;
using SpyFall.Server.Hubs;
using SpyFall.Server.Models;

namespace SpyFall.Server.Services;

public class TimerSyncService(
	GameTimerService timerService,
	VoteService voteService,
	IHubContext<GameHub> hubContext,
	IServiceScopeFactory scopeFactory) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int tick = 0;

		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(1000, stoppingToken);
			tick++;

			foreach (KeyValuePair<string, GameTimerState> entry in timerService.GetAllTimers())
			{
				if (entry.Value.IsPaused) continue;

				int remaining = entry.Value.GetRemainingSeconds();

				if (remaining <= 0)
				{
					await EndExpiredGame(entry.Key, stoppingToken);
				}
				else if (tick % 15 == 0)
				{
					await hubContext.Clients.Group(entry.Key).SendAsync("TimerSync", remaining, stoppingToken);
				}
			}
		}
	}

	private async Task EndExpiredGame(string code, CancellationToken stoppingToken)
	{
		timerService.RemoveTimer(code);
		voteService.RemoveVote(code);

		using IServiceScope scope = scopeFactory.CreateScope();
		AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		Game? game = await db.Games
			.Include(x => x.Players)
			.Include(x => x.Location)
			.FirstOrDefaultAsync(x => x.Code == code, stoppingToken);

		if (game == null) return;

		Player? spy = game.Players.FirstOrDefault(p => p.IsSpy);
		string spyName = spy?.Name ?? "Unknown";
		string locationName = game.Location?.Name ?? "Unknown";

		await hubContext.Clients.Group(code).SendAsync("GameEnded", "SpyWins", locationName, spyName, stoppingToken);

		game.Status = GameStatus.Waiting;
		game.LocationId = null;
		game.LastActivityAt = DateTime.UtcNow;

		foreach (Player player in game.Players)
		{
			player.IsSpy = false;
			player.RoleId = null;
			player.IsReady = false;
		}

		await db.SaveChangesAsync(stoppingToken);
	}
}
