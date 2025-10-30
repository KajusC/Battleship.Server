using System;
using System.Linq;
using System.Text;

namespace Battleship.Server.Models;

public static class ScoreboardFormatter
{
    public static string FormatAsTable(Scoreboard scoreboard, Game game)
    {
        var sb = new StringBuilder();
        sb.AppendLine("╔═══════════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                           BATTLESHIP SCOREBOARD                               ║");
        sb.AppendLine("╠═══════════════════════════════════════════════════════════════════════════════╣");
        sb.AppendLine($"║ Game Phase: {game.Phase,-30} Turn: {GetCurrentPlayerName(game),-30} ║");
        sb.AppendLine("╠═══════════════════════════════════════════════════════════════════════════════╣");

        var scores = scoreboard.GetAllScores().ToList();
        
        if (scores.Count == 0)
        {
            sb.AppendLine("║ No players yet...                                                            ║");
            sb.AppendLine("╚═══════════════════════════════════════════════════════════════════════════════╝");
            return sb.ToString();
        }

        foreach (var score in scores)
        {
            var isCurrentTurn = score.PlayerId == game.CurrentTurnPlayerId;
            var turnIndicator = isCurrentTurn ? "►" : " ";
            
            sb.AppendLine($"║ {turnIndicator} {score.PlayerName,-74} ║");
            sb.AppendLine($"║   Ships: {score.ShipsRemaining}/{score.TotalShips} remaining  |  Shots: {score.TotalShotsFired,-3} ({score.TotalHits} hits, {score.TotalMisses} misses)  |  Accuracy: {score.Accuracy,5:F1}%  ║");
            sb.AppendLine($"║   Enemy Ships Sunk: {score.ShipsSunk}  |  Score: {score.CalculatedScore,-6}                                   ║");
            
            if (score != scores.Last())
            {
                sb.AppendLine("╠───────────────────────────────────────────────────────────────────────────────╣");
            }
        }

        var leader = scoreboard.GetLeadingPlayer();
        if (leader != null && game.Phase == GamePhase.InProgress)
        {
            sb.AppendLine("╠═══════════════════════════════════════════════════════════════════════════════╣");
            sb.AppendLine($"║ Leading Player: {leader.PlayerName,-60} ║");
        }

        sb.AppendLine("╚═══════════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }

    public static string FormatAsCompact(Scoreboard scoreboard, Game game)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== SCOREBOARD ({game.Phase}) ===");

        var scores = scoreboard.GetAllScores().OrderByDescending(s => s.Accuracy).ToList();
        
        foreach (var score in scores)
        {
            var turnIndicator = score.PlayerId == game.CurrentTurnPlayerId ? "►" : " ";
            sb.AppendLine($"{turnIndicator} {score.PlayerName}: {score.TotalHits}/{score.TotalShotsFired} ({score.Accuracy:F1}%) | Ships: {score.ShipsRemaining}/{score.TotalShips}");
        }

        return sb.ToString();
    }

    public static string FormatGameSummary(Scoreboard scoreboard, Game game, string? winnerId)
    {
        var sb = new StringBuilder();
        sb.AppendLine("╔═══════════════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine("║                           GAME OVER - FINAL STATS                             ║");
        sb.AppendLine("╠═══════════════════════════════════════════════════════════════════════════════╣");

        if (winnerId != null)
        {
            var winner = scoreboard.GetPlayerScore(winnerId);
            if (winner != null)
            {
                sb.AppendLine($"║ 🏆 WINNER: {winner.PlayerName,-66} ║");
                sb.AppendLine("╠═══════════════════════════════════════════════════════════════════════════════╣");
            }
        }

        var scores = scoreboard.GetAllScores().OrderByDescending(s => s.CalculatedScore).ToList();
        
        foreach (var score in scores)
        {
            sb.AppendLine($"║ {score.PlayerName,-76} ║");
            sb.AppendLine($"║   Total Shots: {score.TotalShotsFired,-3} | Hits: {score.TotalHits,-3} | Misses: {score.TotalMisses,-3} | Accuracy: {score.Accuracy,5:F1}%    ║");
            sb.AppendLine($"║   Ships Sunk: {score.ShipsSunk} | Ships Lost: {score.TotalShips - score.ShipsRemaining,-1} | Final Score: {score.CalculatedScore,-6}                  ║");
            
            if (score != scores.Last())
            {
                sb.AppendLine("╠───────────────────────────────────────────────────────────────────────────────╣");
            }
        }

        sb.AppendLine("╚═══════════════════════════════════════════════════════════════════════════════╝");

        return sb.ToString();
    }

    private static string GetCurrentPlayerName(Game game)
    {
        if (game.CurrentTurnPlayerId == null)
            return "N/A";

        var player = game.FindPlayerById(game.CurrentTurnPlayerId);
        return player?.Name ?? "Unknown";
    }
}
