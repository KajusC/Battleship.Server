using Battleship.Server.Models.Dtos;
using System.Linq;

namespace Battleship.Server.Models;

public static class ScoreboardExtensions
{
    public static ScoreboardDto ToDto(this Scoreboard scoreboard, Game game)
    {
        var playerScores = scoreboard.GetAllScores()
            .Select(score => score.ToDto(game.CurrentTurnPlayerId))
            .ToList();

        var leadingPlayer = scoreboard.GetLeadingPlayer();

        return new ScoreboardDto(
            playerScores: playerScores,
            currentTurnPlayerId: game.CurrentTurnPlayerId,
            gamePhase: game.Phase,
            leadingPlayerId: leadingPlayer?.PlayerId
        );
    }

    public static PlayerScoreDto ToDto(this PlayerScore score, string? currentTurnPlayerId)
    {
        return new PlayerScoreDto(
            playerId: score.PlayerId,
            playerName: score.PlayerName,
            totalShips: score.TotalShips,
            shipsRemaining: score.ShipsRemaining,
            shipsSunk: score.ShipsSunk,
            totalShotsFired: score.TotalShotsFired,
            totalHits: score.TotalHits,
            totalMisses: score.TotalMisses,
            accuracy: score.Accuracy,
            shipsDestroyedPercentage: score.ShipsDestroyedPercentage,
            calculatedScore: score.CalculatedScore,
            isCurrentTurn: score.PlayerId == currentTurnPlayerId
        );
    }
}
