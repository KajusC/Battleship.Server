namespace Battleship.Server.Models.Dtos;

public sealed class OpponentStatusDto
{
    public string GameId { get; set; } = string.Empty;
    public string OpponentName { get; set; } = string.Empty;
}
