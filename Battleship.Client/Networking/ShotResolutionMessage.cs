namespace Battleship.Client.Networking;

public sealed class ShotResolutionMessage
{
    public string GameId { get; set; } = string.Empty;
    public string ShooterId { get; set; } = string.Empty;
    public string TargetPlayerId { get; set; } = string.Empty;
    public CoordinateMessage Target { get; set; } = new();
    public string Outcome { get; set; } = string.Empty;
    public string? ShipName { get; set; }
    public bool Victory { get; set; }
    public string? NextPlayerId { get; set; }
    public int? WeaponType { get; set; }
}
