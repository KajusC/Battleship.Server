namespace Battleship.Client.Networking;

public sealed class OpponentStatusMessage
{
    public string GameId { get; set; } = string.Empty;
    public string OpponentName { get; set; } = string.Empty;
}
