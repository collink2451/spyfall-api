using System.Collections.Concurrent;

namespace SpyFall.Server.Services;

public class GameTimerService
{
	private readonly ConcurrentDictionary<string, GameTimerState> _timers = new();

	public int StartTimer(string code, int durationSeconds = 600)
	{
		_timers[code] = new GameTimerState
		{
			DurationSeconds = durationSeconds,
			StartedAt = DateTime.UtcNow,
		};
		return durationSeconds;
	}

	public int PauseTimer(string code)
	{
		if (!_timers.TryGetValue(code, out GameTimerState? state) || state.IsPaused)
			return 0;

		state.PausedRemainingSeconds = state.GetRemainingSeconds();
		state.IsPaused = true;
		return state.PausedRemainingSeconds;
	}

	public int ResumeTimer(string code)
	{
		if (!_timers.TryGetValue(code, out GameTimerState? state) || !state.IsPaused)
			return 0;

		// Rewind StartedAt so remaining time matches what was paused
		state.StartedAt = DateTime.UtcNow.AddSeconds(-(state.DurationSeconds - state.PausedRemainingSeconds));
		state.IsPaused = false;
		return state.GetRemainingSeconds();
	}

	public void RemoveTimer(string code)
	{
		_timers.TryRemove(code, out _);
	}

	public GameTimerState? GetTimer(string code) =>
		_timers.TryGetValue(code, out GameTimerState? state) ? state : null;

	public IEnumerable<KeyValuePair<string, GameTimerState>> GetAllTimers() => _timers;
}
