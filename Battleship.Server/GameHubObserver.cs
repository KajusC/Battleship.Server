using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Server.Models;
using Battleship.Server.Models.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Battleship.Server;

public sealed class GameHubObserver : IGameObserver
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly ILogger<GameHubObserver> _logger;

    public GameHubObserver(IHubContext<GameHub> hubContext, ILogger<GameHubObserver> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public void OnPhaseChanged(Game game, GamePhase oldPhase, GamePhase newPhase)
    {
        var payload = new GamePhaseChangedDto
        {
            GameId = game.Id,
            OldPhase = oldPhase.ToString(),
            NewPhase = newPhase.ToString()
        };

        FireAndForget(async () =>
        {
            var broadcasts = new List<Task>
            {
                _hubContext.Clients.Group(game.Id).SendAsync("GamePhaseChanged", payload)
            };

            broadcasts.AddRange(game.Players.Select(player =>
                _hubContext.Clients.Client(player.ConnectionId).SendAsync("GamePhaseChanged", payload)));

            await Task.WhenAll(broadcasts).ConfigureAwait(false);
            
            if (newPhase == GamePhase.InProgress)
            {
                var scoreboardDto = game.Scoreboard.ToDto(game);
                await _hubContext.Clients.Group(game.Id).SendAsync("ScoreboardUpdated", scoreboardDto).ConfigureAwait(false);
                _logger.LogDebug($"Initial scoreboard sent for game {game.Id}");
            }
        });
    }

    public void OnScoreboardChange(Game game)
    {
        var scoreboardDto = game.Scoreboard.ToDto(game);

        FireAndForget(async () =>
        {
            await _hubContext.Clients.Group(game.Id).SendAsync("ScoreboardUpdated", scoreboardDto).ConfigureAwait(false);
            
            _logger.LogDebug($"Scoreboard updated for game {game.Id} after shot");
        });
    }

    private void FireAndForget(Func<Task> action)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await action().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to broadcast observer notification");
            }
        });
    }
}
