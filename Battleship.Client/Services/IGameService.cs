using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Battleship.Client.Networking;

namespace Battleship.Client.Services;

public interface IGameService : IAsyncDisposable
{
    Task ConnectAsync();
    Task<JoinGameResponseMessage> JoinGameAsync(string playerName, bool isSinglePlayer);
    Task<PlacementResultMessage> SubmitPlacementsAsync(IEnumerable<ShipPlacementDto> placements);
    Task<ShotResolutionMessage> FireAsync(string gameId, int row, int column);
    Task DisconnectAsync();

    event Action<WaitingStatusMessage>? WaitingForOpponent;
    event Action<OpponentStatusMessage>? OpponentJoined;
    event Action<OpponentStatusMessage>? OpponentReady;
    event Action<GameStartMessage>? PlacementPhaseStarted;
    event Action<PlacementResultMessage>? PlacementConfirmed;
    event Action<GameStartMessage>? GameStarted;
    event Action<ShotResolutionMessage>? ShotResolved;
    event Action<GameEndedMessage>? GameEnded;
    event Action<TurnChangedMessage>? TurnChanged;
    event Action<WaitingStatusMessage>? OpponentDisconnected;
    event Action<ErrorMessage>? ErrorReceived;
}
