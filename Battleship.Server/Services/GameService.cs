using System;
using System.Collections.Generic;
using System.Linq;
using Battleship.Server.Models;
using Battleship.Server.Models.Builder;

namespace Battleship.Server.Services;

public sealed class GameService
{
    private static readonly Lazy<GameService> _instance = new(() => new GameService());
    private readonly object _syncRoot = new();
    private readonly Dictionary<string, Game> _games = new();
    private readonly Dictionary<string, Game> _gamesByConnection = new();
    private Game? _waitingGame;
    private readonly List<IGameObserver> _observers = new();

    private GameService()
    {
    }

    public static GameService Instance => _instance.Value;

    public static void RegisterObservers(IEnumerable<IGameObserver> observers)
    {
        if (observers is null)
        {
            return;
        }

        foreach (var observer in observers)
        {
            Instance.RegisterObserver(observer);
        }
    }

    public void RegisterObserver(IGameObserver observer)
    {
        lock (_syncRoot)
        {
            if (_observers.Contains(observer))
            {
                return;
            }

            _observers.Add(observer);
            foreach (var game in _games.Values)
            {
                game.Subscribe(observer);
            }

            _waitingGame?.Subscribe(observer);
        }
    }

    public JoinGameResult JoinGame(string connectionId, string playerName)
    {
        return JoinGame(connectionId, playerName, GameMode.standard);
    }

    public JoinGameResult JoinGame(string connectionId, string playerName, GameMode gameMode)
    {
        if (string.IsNullOrWhiteSpace(playerName))
        {
            throw new ArgumentException("Player name is required", nameof(playerName));
        }

        lock (_syncRoot)
        {
            Game game;
            if (_waitingGame is null || _waitingGame.Players.Count >= 2)
            {
                _waitingGame = CreateGame(gameMode);
            }

            game = _waitingGame;
            var player = game.AddPlayer(connectionId, playerName);
            _games[game.Id] = game;
            _gamesByConnection[connectionId] = game;

            if (game.Players.Count == 2)
            {
                _waitingGame = null;
            }

            return new JoinGameResult(game, player, game.Players.Count == 1);
        }
    }

    public Game? GetGameByConnection(string connectionId)
    {
        lock (_syncRoot)
        {
            _gamesByConnection.TryGetValue(connectionId, out var game);
            return game;
        }
    }

    public Game? GetGameById(string gameId)
    {
        if (string.IsNullOrWhiteSpace(gameId))
        {
            return null;
        }

        lock (_syncRoot)
        {
            _games.TryGetValue(gameId, out var game);
            return game;
        }
    }

    public GamePlayer? GetPlayerByConnection(string connectionId)
    {
        lock (_syncRoot)
        {
            if (_gamesByConnection.TryGetValue(connectionId, out var game))
            {
                return game.FindPlayerByConnection(connectionId);
            }
            return null;
        }
    }

    public void ApplyPlacements(string connectionId, IEnumerable<ShipPlacement> placements)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(connectionId);
            var player = game.FindPlayerByConnection(connectionId) ?? throw new InvalidOperationException("Player not found in game.");
            game.ApplyPlacements(player, placements);
        }
    }

    public FireResult Fire(string connectionId, Coordinate coordinate)
    {
        lock (_syncRoot)
        {
            var game = GetGameOrThrow(connectionId);
            var shooter = game.FindPlayerByConnection(connectionId) ?? throw new InvalidOperationException("Player not found in game.");
            return game.Fire(shooter, coordinate);
        }
    }

    public IReadOnlyDictionary<ShipType, int> GetFleetDefinition()
    {
        return ShipSpecification.StandardFleet;
    }

    public Game? RemovePlayer(string connectionId)
    {
        lock (_syncRoot)
        {
            if (!_gamesByConnection.TryGetValue(connectionId, out var game))
            {
                return null;
            }

            game.RemovePlayer(connectionId);
            _gamesByConnection.Remove(connectionId);
            if (game.Players.Count == 0)
            {
                _games.Remove(game.Id);
            }
            else if (game.Phase != GamePhase.Completed)
            {
                // game over
            }

            return game;
        }
    }

    private Game GetGameOrThrow(string connectionId)
    {
        if (!_gamesByConnection.TryGetValue(connectionId, out var game))
        {
            throw new InvalidOperationException("Player is not part of any game.");
        }

        return game;
    }

    private void AttachObservers(Game game)
    {
        foreach (var observer in _observers)
        {
            game.Subscribe(observer);
        }
    }

    private Game CreateGame(GameMode gameMode)
    {
        var template = GameConfigurationPrototypes.GetByMode(gameMode);
        var game = new Game(template);
        AttachObservers(game);
        return game;
    }
}

public sealed record JoinGameResult(Game Game, GamePlayer Player, bool IsWaitingForOpponent);
