namespace SpyFall.Server.Models;

public class Role
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public int LocationId { get; set; }
	public Location Location { get; set; } = null!;
}
