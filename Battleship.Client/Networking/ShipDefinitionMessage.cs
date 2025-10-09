namespace Battleship.Client.Networking;

public sealed class ShipDefinitionMessage
{
    public string Name { get; set; } = string.Empty;
    public int Length { get; set; }
}
