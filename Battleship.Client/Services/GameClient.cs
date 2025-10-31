using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Client.Networking;
using Microsoft.AspNetCore.SignalR.Client;

namespace Battleship.Client.Services;

public sealed class GameClient : IAsyncDisposable
{
    private readonly HubConnection _connection;

    public GameClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        RegisterHandlers();
    }

    public string? PlayerId { get; private set; }
    public string? GameId { get; private set; }
    public int BoardSize { get; private set; }

    public event Action<WaitingStatusMessage>? WaitingForOpponent;
    public event Action<OpponentStatusMessage>? OpponentJoined;
    public event Action<OpponentStatusMessage>? OpponentReady;
    public event Action<GameStartMessage>? PlacementPhaseStarted;
    public event Action<GamePhaseChangedMessage>? GamePhaseChanged;
    public event Action<PlacementResultMessage>? PlacementConfirmed;
    public event Action<GameStartMessage>? GameStarted;
    public event Action<ShotResolutionMessage>? ShotResolved;
    public event Action<GameEndedMessage>? GameEnded;
    public event Action<TurnChangedMessage>? TurnChanged;
    public event Action<WaitingStatusMessage>? OpponentDisconnected;
    public event Action<ErrorMessage>? ErrorReceived;
    public event Action<ScoreboardMessage>? ScoreboardUpdated;

    public Task ConnectAsync() => _connection.StartAsync();

    public async Task<JoinGameResponseMessage> JoinGameAsync(string playerName, bool IsSinglePlayer)
    {
        return await JoinGameAsync(playerName, IsSinglePlayer, "standard");
    }

    public async Task<JoinGameResponseMessage> JoinGameAsync(string playerName, bool IsSinglePlayer, string gameMode)
    {
        var response = await _connection.InvokeAsync<JoinGameResponseMessage>("JoinGame", playerName, IsSinglePlayer, gameMode);
        PlayerId = response.PlayerId;
        GameId = response.GameId;
        BoardSize = response.BoardSize;
        return response;
    }

    public Task<PlacementResultMessage> SubmitPlacementsAsync(IEnumerable<ShipPlacementDto> placements)
    {
        return _connection.InvokeAsync<PlacementResultMessage>("PlaceShips", placements);
    }

    public Task<ShotResolutionMessage> FireAsync(string gameId, int row, int column)
    {
        var request = new ShotRequestDto
        {
            GameId = gameId,
            Row = row,
            Column = column
        };
        return _connection.InvokeAsync<ShotResolutionMessage>("Fire", request);
    }

    public Task DisconnectAsync() => _connection.StopAsync();

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    private void RegisterHandlers()
    {
        _connection.On<WaitingStatusMessage>("WaitingForOpponent", message => WaitingForOpponent?.Invoke(message));
        _connection.On<OpponentStatusMessage>("OpponentJoined", message => OpponentJoined?.Invoke(message));
        _connection.On<OpponentStatusMessage>("OpponentReady", message => OpponentReady?.Invoke(message));
        _connection.On<GameStartMessage>("PlacementPhaseStarted", message => PlacementPhaseStarted?.Invoke(message));
        _connection.On<GamePhaseChangedMessage>("GamePhaseChanged", message => GamePhaseChanged?.Invoke(message));
        _connection.On<PlacementResultMessage>("PlacementConfirmed", message => PlacementConfirmed?.Invoke(message));
        _connection.On<GameStartMessage>("GameStarted", message => GameStarted?.Invoke(message));
        _connection.On<ShotResolutionMessage>("ShotResolved", message => ShotResolved?.Invoke(message));
        _connection.On<GameEndedMessage>("GameEnded", message => GameEnded?.Invoke(message));
        _connection.On<TurnChangedMessage>("TurnChanged", message => TurnChanged?.Invoke(message));
        _connection.On<WaitingStatusMessage>("OpponentDisconnected", message => OpponentDisconnected?.Invoke(message));
        _connection.On<ErrorMessage>("Error", message => ErrorReceived?.Invoke(message));
        _connection.On<ScoreboardMessage>("ScoreboardUpdated", message => { if (message != null) ScoreboardUpdated?.Invoke(message); });
    }
}

public sealed class ShipPlacementDto
{
    public string ShipName { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public string Orientation { get; set; } = string.Empty;
}

public sealed class ShotRequestDto
{
    public string GameId { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
}
