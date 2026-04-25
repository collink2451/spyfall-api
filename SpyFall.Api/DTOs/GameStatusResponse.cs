namespace SpyFall.Api.DTOs;

public class GameStatusResponse
{
	public string Code { get; set; } = string.Empty;
	public string Status { get; set; } = string.Empty;
	public List<string> Players { get; set; } = [];
}
