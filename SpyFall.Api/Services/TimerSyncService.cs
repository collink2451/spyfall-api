using Microsoft.AspNetCore.SignalR;
using SpyFall.Api.Hubs;

namespace SpyFall.Api.Services;

public class TimerSyncService(GameTimerService timerService, IHubContext<GameHub> hubContext) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(15000, stoppingToken);

			foreach (KeyValuePair<string, GameTimerState> entry in timerService.GetAllTimers())
			{
				if (entry.Value.IsPaused) continue;
				int remaining = entry.Value.GetRemainingSeconds();
				await hubContext.Clients.Group(entry.Key).SendAsync("TimerSync", remaining, stoppingToken);
			}
		}
	}
}
