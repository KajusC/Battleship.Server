using Battleship.Server.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using static Battleship.Server.Models.Game;

namespace Battleship.Server.Models;

public enum GamePhase
{
    WaitingForPlayers,
    Placement,
    InProgress,
    Completed
}

public interface IGameObserver
{
    void OnPhaseChanged(Game game, GamePhase oldPhase, GamePhase newPhase);
    void OnTurnChanged(Game game, string? oldPlayerId, string? newPlayerId);
}


public sealed class Game
{
    private GamePhase _phase;
    private readonly List<IGameObserver> _observers = new();
    private readonly Dictionary<string, GamePlayer> _players = new();
    private readonly Dictionary<string, GamePlayer> _playersByConnection = new();
    public readonly int _boardSize;

    public GamePhase Phase
    {
        get => _phase;
        private set
        {
            if (_phase != value)
            {
                var oldPhase = _phase;
                _phase = value;
                NotifyPhaseChanged(oldPhase, value);
            }
        }
    }

    public void Subscribe(IGameObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IGameObserver observer)
    {
        _observers.Remove(observer);
    }

    public Game(int boardSize)
    {
        _boardSize = boardSize;
        Id = Guid.NewGuid().ToString("N");
    }

    public string Id { get; }
    public string? CurrentTurnPlayerId { get; private set; }

    public IReadOnlyCollection<GamePlayer> Players => _players.Values;

    public GamePlayer AddPlayer(string connectionId, string name)
    {
        if (_players.Count >= 2)
        {
            throw new InvalidOperationException("Game already has two players.");
        }

        var playerId = Guid.NewGuid().ToString("N");
        var player = new GamePlayer(playerId, connectionId, name, new Board(_boardSize));
        _players.Add(playerId, player);
        _playersByConnection.Add(connectionId, player);

        if (_players.Count == 2)
        {
            Phase = GamePhase.Placement;
        }

        return player;
    }

    public bool ContainsConnection(string connectionId) => _playersByConnection.ContainsKey(connectionId);

    public GamePlayer? FindPlayerByConnection(string connectionId)
    {
        _playersByConnection.TryGetValue(connectionId, out var player);
        return player;
    }

    public GamePlayer? FindPlayerById(string playerId)
    {
        _players.TryGetValue(playerId, out var player);
        return player;
    }

    public GamePlayer? GetOpponent(string playerId)
    {
        return _players.Values.FirstOrDefault(p => p.PlayerId != playerId);
    }

    public void RemovePlayer(string connectionId)
    {
        if (_playersByConnection.TryGetValue(connectionId, out var player))
        {
            _playersByConnection.Remove(connectionId);
            _players.Remove(player.PlayerId);

            if (_players.Count < 2 && Phase != GamePhase.Completed)
            {
                Phase = GamePhase.WaitingForPlayers;
            }
        }
    }

    public IReadOnlyDictionary<string, int> FleetDefinition => ShipSpecification.StandardFleet;

    public void ApplyPlacements(GamePlayer player, IEnumerable<ShipPlacement> placements)
    {
        if (Phase != GamePhase.Placement)
        {
            throw new InvalidOperationException("Game is not in placement phase.");
        }

        var placementsList = placements.ToList();
        ValidatePlacements(placementsList);

        var ships = placementsList.Select(p => BuildShip(p)).ToList();
        player.Board.PlaceShips(ships);

        player.SetReady();

        if (Players.All(p => p.IsReady))
        {
            StartGame();
            return;
        }


    }

    public FireResult Fire(GamePlayer shooter, Coordinate coordinate)
    {
        if (Phase != GamePhase.InProgress)
        {
            throw new InvalidOperationException("Game is not active.");
        }

        if (CurrentTurnPlayerId != shooter.PlayerId)
        {
            throw new InvalidOperationException("It is not the player's turn.");
        }

        var target = GetOpponent(shooter.PlayerId) ?? throw new InvalidOperationException("Opponent not found.");
        var resolution = target.Board.Shoot(coordinate);
        var isVictory = target.Board.AreAllShipsSunk;

        if (isVictory)
        {
            Phase = GamePhase.Completed;
            CurrentTurnPlayerId = shooter.PlayerId;
        }
        else if (resolution.Outcome.Equals(ShotOutcome.Hit) || 
                resolution.Outcome.Equals(ShotOutcome.Sunk))
        {
            CurrentTurnPlayerId = shooter.PlayerId;
        }
        else
        {
            CurrentTurnPlayerId = target.PlayerId;
        }
        var result = new FireResult(shooter.PlayerId, target.PlayerId, coordinate, resolution, isVictory, CurrentTurnPlayerId);

        NotifyTurnChanged(shooter.PlayerId, CurrentTurnPlayerId);

        return result;

    }

    private void StartGame()
    {
        Phase = GamePhase.InProgress;
        var random = new Random();
        var players = Players.ToList();

        var oldTurnPlayerId = CurrentTurnPlayerId;
        CurrentTurnPlayerId = players[random.Next(players.Count)].PlayerId;

        NotifyTurnChanged(oldTurnPlayerId, CurrentTurnPlayerId);

    }

    private void ValidatePlacements(IReadOnlyCollection<ShipPlacement> placements)
    {
        if (placements.Count != ShipSpecification.StandardFleet.Count)
        {
            throw new InvalidOperationException("Invalid number of ships placed.");
        }

        var names = placements.Select(p => p.Name).ToList();
        if (names.Distinct().Count() != ShipSpecification.StandardFleet.Count)
        {
            throw new InvalidOperationException("Ship names must be unique.");
        }

        foreach (var placement in placements)
        {
            if (!ShipSpecification.StandardFleet.TryGetValue(placement.Name, out var expectedLength))
            {
                throw new InvalidOperationException($"Unknown ship type '{placement.Name}'.");
            }

            var actualLength = expectedLength;
            var coordinates = GenerateCoordinates(placement, actualLength).ToList();
            if (coordinates.Count != expectedLength)
            {
                throw new InvalidOperationException($"Ship '{placement.Name}' has invalid length.");
            }
        }
    }

    private Ship BuildShip(ShipPlacement placement)
    {
        var length = ShipSpecification.StandardFleet[placement.Name];
        var coordinates = GenerateCoordinates(placement, length).ToList();
        return new Ship(placement.Name, coordinates);
    }

    private IEnumerable<Coordinate> GenerateCoordinates(ShipPlacement placement, int length)
    {
        for (var i = 0; i < length; i++)
        {
            var row = placement.Orientation == ShipOrientation.Horizontal ? placement.Start.Row : placement.Start.Row + i;
            var column = placement.Orientation == ShipOrientation.Horizontal ? placement.Start.Column + i : placement.Start.Column;
            var coordinate = new Coordinate(row, column);
            if (!coordinate.IsWithin(_boardSize))
            {
                throw new InvalidOperationException("Ship placement is outside of the board.");
            }

            yield return coordinate;
        }
    }

    private void NotifyPhaseChanged(GamePhase oldPhase, GamePhase newPhase)
    {
        foreach (var observer in _observers)
        {
            observer.OnPhaseChanged(this, oldPhase, newPhase);
        }
    }

    private void NotifyTurnChanged(string? oldPlayerId, string? newPlayerId)
    {
        foreach (var observer in _observers)
        {
            observer.OnTurnChanged(this, oldPlayerId, newPlayerId);
        }
    }
}

public sealed record FireResult(
    string ShooterId,
    string TargetPlayerId,
    Coordinate Coordinate,
    ShotResolution Resolution,
    bool IsVictory,
    string? NextPlayerId);


public class GamePlayer
{
    public GamePlayer(string playerId, string connectionId, string name, Board board)
    {
        PlayerId = playerId;
        ConnectionId = connectionId;
        Name = name;
        Board = board;
    }

    public string PlayerId { get; }
    public string ConnectionId { get; }
    public string Name { get; }
    public Board Board { get; }
    public bool IsReady { get; private set; }

    public void SetReady() => IsReady = true;
}

