namespace Battleship.Server.Models;

public sealed class ShipPlacement
{
    public ShipPlacement(string name, Coordinate start, ShipOrientation orientation)
    {
        Name = name;
        Start = start;
        Orientation = orientation;
    }

    public string Name { get; }
    public Coordinate Start { get; }
    public ShipOrientation Orientation { get; }
}
