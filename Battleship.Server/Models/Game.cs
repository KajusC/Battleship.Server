using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Server.Models;

public sealed class Game
{
    private readonly Dictionary<string, GamePlayer> _players = new();
    private readonly Dictionary<string, GamePlayer> _playersByConnection = new();
    private readonly int _boardSize;

    public Game(int boardSize)
    {
        _boardSize = boardSize;
        Id = Guid.NewGuid().ToString("N");
        Phase = GamePhase.WaitingForPlayers;
    }

    public string Id { get; }
    public GamePhase Phase { get; private set; }
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
        else
        {
            CurrentTurnPlayerId = target.PlayerId;
        }

        return new FireResult(shooter.PlayerId, target.PlayerId, coordinate, resolution, isVictory, CurrentTurnPlayerId);
    }

    private void StartGame()
    {
        Phase = GamePhase.InProgress;
        var random = new Random();
        var players = Players.ToList();
        CurrentTurnPlayerId = players[random.Next(players.Count)].PlayerId;
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
}

public sealed record FireResult(
    string ShooterId,
    string TargetPlayerId,
    Coordinate Coordinate,
    ShotResolution Resolution,
    bool IsVictory,
    string? NextPlayerId);
