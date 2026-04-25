namespace SpyFall.Api.Services;

public class GameTimerState
{
	public int DurationSeconds { get; init; }
	public DateTime StartedAt { get; set; }
	public int PausedRemainingSeconds { get; set; }
	public bool IsPaused { get; set; }

	public int GetRemainingSeconds()
	{
		if (IsPaused) return PausedRemainingSeconds;
		int elapsed = (int)(DateTime.UtcNow - StartedAt).TotalSeconds;
		return Math.Max(0, DurationSeconds - elapsed);
	}
}
