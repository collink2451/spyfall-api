using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;
using SpyFall.Api.DTOs;
using SpyFall.Api.Models;

namespace SpyFall.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class GamesController(AppDbContext db) : ControllerBase
{
	private readonly AppDbContext mDb = db;

	[HttpPost]
	public async Task<IActionResult> CreateGame()
	{
		string gameCode;
		do
		{
			gameCode = Guid.NewGuid().ToString("N")[..6].ToUpper();
		}
		while (await mDb.Games.AnyAsync(g => g.Code == gameCode));

		Game game = new()
		{
			Status = GameStatus.Waiting,
			Code = gameCode,
			LastActivityAt = DateTime.UtcNow
		};

		mDb.Games.Add(game);
		await mDb.SaveChangesAsync();

		return Ok(gameCode);
	}

	[HttpPost("{code}/join")]
	public async Task<IActionResult> JoinGame([FromRoute] string code, [FromBody] JoinGameRequest request)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return BadRequest("Game not found");
		if (game.Status != GameStatus.Waiting) return BadRequest("Game already in progress");

		string name = request.Name.Trim();
		if (string.IsNullOrEmpty(name)) return BadRequest("Name cannot be empty");
		if (name.Length > 20) return BadRequest("Name cannot exceed 20 characters");

		if (game.Players.Any(x => x.Name == name)) return BadRequest("Name in use");
		if (game.Players.Count >= 10) return BadRequest("Game is full");

		Player player = new()
		{
			Name = name
		};

		game.Players.Add(player);

		try
		{
			await mDb.SaveChangesAsync();
		}
		catch (DbUpdateException)
		{
			return BadRequest("Name in use");
		}

		return Ok(player.Id);
	}

	[HttpGet("{code}")]
	public async Task<IActionResult> GetStatus([FromRoute] string code)
	{
		Game? game = await mDb.Games
			.Include(x => x.Players)
			.FirstOrDefaultAsync(x => x.Code == code);

		if (game == null) return BadRequest("Game not found");

		return Ok(new GameStatusResponse
		{
			Code = game.Code,
			Status = game.Status.ToString(),
			HostPlayerId = game.HostPlayerId,
			Players = [.. game.Players.Select(p => new PlayerResponse
			{
				Id = p.Id,
				Name = p.Name,
				IsReady = p.IsReady
			})]
		});
	}
}
