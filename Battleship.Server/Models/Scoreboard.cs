namespace Battleship.Server.Models;

public sealed class Scoreboard
{
    private readonly Dictionary<string, PlayerScore> _playerScores = new();

    public Scoreboard()
    {
    }

    public void Initialize(Game game)
    {
        _playerScores.Clear();
        foreach (var player in game.Players)
        {
            var score = new PlayerScore(
                playerId: player.PlayerId,
                playerName: player.Name,
                totalShips: player.Board.Ships.Count,
                shipsRemaining: player.Board.Ships.Count(s => !s.IsSunk)
            );
            _playerScores[player.PlayerId] = score;
        }
    }

    public void RecordShot(string shooterId, string targetId, ShotOutcome outcome)
    {
        if (_playerScores.TryGetValue(shooterId, out var shooterScore))
        {
            shooterScore.TotalShotsFired++;
            
            if (outcome == ShotOutcome.Hit || outcome == ShotOutcome.Sunk)
            {
                shooterScore.TotalHits++;
            }
            else if (outcome == ShotOutcome.Miss)
            {
                shooterScore.TotalMisses++;
            }
        }

        if (_playerScores.TryGetValue(targetId, out var targetScore) && outcome == ShotOutcome.Sunk)
        {
            targetScore.ShipsRemaining--;
            targetScore.ShipsSunk++;
        }
    }

    public PlayerScore? GetPlayerScore(string playerId)
    {
        return _playerScores.TryGetValue(playerId, out var score) ? score : null;
    }

    public IReadOnlyCollection<PlayerScore> GetAllScores()
    {
        return _playerScores.Values.ToList().AsReadOnly();
    }

    public PlayerScore? GetLeadingPlayer()
    {
        return _playerScores.Values
            .OrderByDescending(s => s.Accuracy)
            .ThenByDescending(s => s.TotalHits)
            .FirstOrDefault();
    }

    public void Clear()
    {
        _playerScores.Clear();
    }
}

public sealed class PlayerScore
{
    public PlayerScore(string playerId, string playerName, int totalShips, int shipsRemaining)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        TotalShips = totalShips;
        ShipsRemaining = shipsRemaining;
    }

    public string PlayerId { get; }
    public string PlayerName { get; }
    public int TotalShips { get; }
    public int ShipsRemaining { get; set; }
    public int ShipsSunk { get; set; }
    public int TotalShotsFired { get; set; }
    public int TotalHits { get; set; }
    public int TotalMisses { get; set; }

    public double Accuracy => TotalShotsFired > 0 
        ? Math.Round((double)TotalHits / TotalShotsFired * 100, 2) 
        : 0.0;

    public double ShipsDestroyedPercentage => TotalShips > 0 
        ? Math.Round((double)ShipsSunk / TotalShips * 100, 2) 
        : 0.0;

    public int CalculatedScore => (TotalHits * 10) + (ShipsRemaining * 50);

    public override string ToString()
    {
        return $"{PlayerName}: {TotalHits}/{TotalShotsFired} hits ({Accuracy}%), {ShipsRemaining}/{TotalShips} ships remaining";
    }
}
