namespace Battleship.Client.Networking;

public sealed class TurnChangedMessage
{
    public string GameId { get; set; } = string.Empty;
    public string CurrentPlayerId { get; set; } = string.Empty;
}
