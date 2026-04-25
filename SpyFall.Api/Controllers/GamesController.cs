using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;
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
}
