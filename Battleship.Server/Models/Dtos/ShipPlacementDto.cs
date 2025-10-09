namespace Battleship.Server.Models.Dtos;

public sealed class ShipPlacementDto
{
    public string ShipName { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public string Orientation { get; set; } = string.Empty;
}
