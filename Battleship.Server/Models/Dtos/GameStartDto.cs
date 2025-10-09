namespace Battleship.Server.Models.Dtos;

public sealed class GameStartDto
{
    public string GameId { get; set; } = string.Empty;
    public string CurrentPlayerId { get; set; } = string.Empty;
    public IReadOnlyCollection<ShipDefinitionDto> Fleet { get; set; } = Array.Empty<ShipDefinitionDto>();
}
