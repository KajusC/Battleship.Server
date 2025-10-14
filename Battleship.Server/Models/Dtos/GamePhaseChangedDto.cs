namespace Battleship.Server.Models.Dtos;

public sealed class GamePhaseChangedDto
{
    public string GameId { get; init; } = string.Empty;
    public string OldPhase { get; init; } = string.Empty;
    public string NewPhase { get; init; } = string.Empty;
}
