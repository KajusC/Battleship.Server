using System.Collections.Generic;

namespace Battleship.Client.Networking;

public sealed class ScoreboardMessage
{
    public IEnumerable<PlayerScoreMessage> PlayerScores { get; set; } = new List<PlayerScoreMessage>();
    public string? CurrentTurnPlayerId { get; set; }
    public string GamePhase { get; set; } = string.Empty;
    public string? LeadingPlayerId { get; set; }
}

public sealed class PlayerScoreMessage
{
    public string PlayerId { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public int TotalShips { get; set; }
    public int ShipsRemaining { get; set; }
    public int ShipsSunk { get; set; }
    public int TotalShotsFired { get; set; }
    public int TotalHits { get; set; }
    public int TotalMisses { get; set; }
    public double Accuracy { get; set; }
    public double ShipsDestroyedPercentage { get; set; }
    public int CalculatedScore { get; set; }
    public bool IsCurrentTurn { get; set; }
}



