namespace Battleship.Server.Models;

internal sealed class BoardCell
{
    public Ship? Ship { get; set; }
    public bool IsShot { get; set; }
}
