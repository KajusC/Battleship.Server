namespace Battleship.Server.Models.Dtos;

public sealed class TurnChangedDto
{
    public string GameId { get; set; } = string.Empty;
    public string CurrentPlayerId { get; set; } = string.Empty;
}
