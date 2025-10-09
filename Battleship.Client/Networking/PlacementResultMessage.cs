namespace Battleship.Client.Networking;

public sealed class PlacementResultMessage
{
    public string GameId { get; set; } = string.Empty;
    public bool AllPlayersReady { get; set; }
    public string Message { get; set; } = string.Empty;
}
