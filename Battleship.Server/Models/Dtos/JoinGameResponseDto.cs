namespace Battleship.Server.Models.Dtos;

public sealed class JoinGameResponseDto
{
    public string GameId { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public int BoardSize { get; set; }
    public bool IsWaitingForOpponent { get; set; }
    public string Message { get; set; } = string.Empty;
}
