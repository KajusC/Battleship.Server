namespace Battleship.Server.Models.Dtos;

public sealed class ShotRequestDto
{
    public string GameId { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public int WeaponType { get; set; } = 0;
}
