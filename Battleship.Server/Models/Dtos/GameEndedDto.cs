namespace Battleship.Server.Models.Dtos;

public sealed class GameEndedDto
{
    public string GameId { get; set; } = string.Empty;
    public string WinnerPlayerId { get; set; } = string.Empty;
    public string WinnerName { get; set; } = string.Empty;
}
