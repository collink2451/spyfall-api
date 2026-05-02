namespace SpyFall.Server.DTOs;

public class GameStatusResponse
{
	public string Code { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public int? HostPlayerId { get; set; }
	public List<PlayerResponse> Players { get; set; } = [];
}
