namespace Battleship.Client.Networking;

public sealed class GamePhaseChangedMessage
{
    public string GameId { get; init; } = string.Empty;
    public string OldPhase { get; init; } = string.Empty;
    public string NewPhase { get; init; } = string.Empty;
}
