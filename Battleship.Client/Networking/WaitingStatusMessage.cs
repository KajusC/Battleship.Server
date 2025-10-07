namespace Battleship.Client.Networking;

public sealed class WaitingStatusMessage
{
    public string GameId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
