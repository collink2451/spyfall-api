using Microsoft.EntityFrameworkCore;
using SpyFall.Api.Data;

namespace SpyFall.Api.Services;

public class GameCleanupService(IServiceScopeFactory scopeFactory, ILogger<GameCleanupService> logger) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

			try
			{
				using IServiceScope scope = scopeFactory.CreateScope();
				AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

				DateTime cutoff = DateTime.UtcNow.AddHours(-24);

				int deleted = await db.Games
					.Where(g => g.LastActivityAt < cutoff)
					.ExecuteDeleteAsync(stoppingToken);

				if (deleted > 0)
					logger.LogInformation("Cleaned up {Count} stale game(s)", deleted);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error during game cleanup");
			}
		}
	}
}
