namespace Battleship.Server.Models;

public readonly record struct Coordinate(int Row, int Column)
{
    public bool IsWithin(int size) => Row >= 0 && Column >= 0 && Row < size && Column < size;
}
