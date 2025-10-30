using System.Collections.Generic;

namespace Battleship.Server.Models.Dtos;

public sealed class ScoreboardDto
{
    public ScoreboardDto(
        IEnumerable<PlayerScoreDto> playerScores,
        string? currentTurnPlayerId,
        GamePhase gamePhase,
        string? leadingPlayerId)
    {
        PlayerScores = playerScores;
        CurrentTurnPlayerId = currentTurnPlayerId;
        GamePhase = gamePhase.ToString();
        LeadingPlayerId = leadingPlayerId;
    }

    public IEnumerable<PlayerScoreDto> PlayerScores { get; }
    public string? CurrentTurnPlayerId { get; }
    public string GamePhase { get; }
    public string? LeadingPlayerId { get; }
}

public sealed class PlayerScoreDto
{
    public PlayerScoreDto(
        string playerId,
        string playerName,
        int totalShips,
        int shipsRemaining,
        int shipsSunk,
        int totalShotsFired,
        int totalHits,
        int totalMisses,
        double accuracy,
        double shipsDestroyedPercentage,
        int calculatedScore,
        bool isCurrentTurn)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        TotalShips = totalShips;
        ShipsRemaining = shipsRemaining;
        ShipsSunk = shipsSunk;
        TotalShotsFired = totalShotsFired;
        TotalHits = totalHits;
        TotalMisses = totalMisses;
        Accuracy = accuracy;
        ShipsDestroyedPercentage = shipsDestroyedPercentage;
        CalculatedScore = calculatedScore;
        IsCurrentTurn = isCurrentTurn;
    }

    public string PlayerId { get; }
    public string PlayerName { get; }
    public int TotalShips { get; }
    public int ShipsRemaining { get; }
    public int ShipsSunk { get; }
    public int TotalShotsFired { get; }
    public int TotalHits { get; }
    public int TotalMisses { get; }
    public double Accuracy { get; }
    public double ShipsDestroyedPercentage { get; }
    public int CalculatedScore { get; }
    public bool IsCurrentTurn { get; }
}
