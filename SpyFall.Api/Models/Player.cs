namespace SpyFall.Api.Models;

public class Player
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string ConnectionId { get; set; } = string.Empty;
	public bool IsSpy { get; set; } = false;
	public bool IsReady { get; set; } = false;

	public int GameId { get; set; }
	public Game Game { get; set; } = null!;

	public int? RoleId { get; set; }
	public Role? Role { get; set; }
}
