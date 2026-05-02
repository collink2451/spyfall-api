namespace SpyFall.Server.DTOs;

public class PlayerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsReady { get; set; }
}
