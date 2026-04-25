namespace SpyFall.Api.Models;

public class Game
{
	public int Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public GameStatus Status { get; set; } = GameStatus.Waiting;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

	public int? LocationId { get; set; }
	public Location? Location { get; set; }

	public ICollection<Player> Players { get; set; } = [];
}
