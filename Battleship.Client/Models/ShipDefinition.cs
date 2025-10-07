namespace Battleship.Client.Models;

public sealed class ShipDefinition
{
    public ShipDefinition(string name, int length)
    {
        Name = name;
        Length = length;
    }

    public string Name { get; }
    public int Length { get; }
}
