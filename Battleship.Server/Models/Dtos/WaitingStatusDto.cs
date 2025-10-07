namespace Battleship.Server.Models.Dtos;

public sealed class WaitingStatusDto
{
    public string GameId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
