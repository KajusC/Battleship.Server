namespace Battleship.Client.Networking;

public sealed class GameEndedMessage
{
    public string GameId { get; set; } = string.Empty;
    public string WinnerPlayerId { get; set; } = string.Empty;
    public string WinnerName { get; set; } = string.Empty;
}
