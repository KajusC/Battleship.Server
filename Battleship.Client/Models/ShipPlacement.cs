namespace Battleship.Client.Models;

public sealed class ShipPlacement
{
    public ShipPlacement(string name, int row, int column, ShipOrientation orientation)
    {
        Name = name;
        Row = row;
        Column = column;
        Orientation = orientation;
    }

    public string Name { get; }
    public int Row { get; }
    public int Column { get; }
    public ShipOrientation Orientation { get; set; }
}
