namespace Battleship.Server.Models.Dtos;

public sealed class PlacementResultDto
{
    public string GameId { get; set; } = string.Empty;
    public bool AllPlayersReady { get; set; }
    public string Message { get; set; } = string.Empty;
}
