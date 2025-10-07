namespace Battleship.Client.Networking;

public sealed class GameStartMessage
{
    public string GameId { get; set; } = string.Empty;
    public string CurrentPlayerId { get; set; } = string.Empty;
    public IReadOnlyCollection<ShipDefinitionMessage> Fleet { get; set; } = Array.Empty<ShipDefinitionMessage>();
}
