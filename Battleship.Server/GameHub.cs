using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleship.Server.Models;
using Battleship.Server.Models.Dtos;
using Battleship.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Battleship.Server;

public class GameHub : Hub
{
    private readonly GameService _gameService;
    private readonly BotCommandQueue _botQueue;

    public GameHub(BotCommandQueue botQueue)
    {
        _botQueue = botQueue;
        _gameService = GameService.Instance;
    }

    public async Task<JoinGameResponseDto> JoinGame(string playerName, bool IsSinglePlayer, string? gameMode = "standard")
    {
        try
        {
            if (!Enum.TryParse<GameMode>(gameMode, true, out var mode))
            {
                mode = GameMode.standard;
            }

            var result = _gameService.JoinGame(Context.ConnectionId, playerName.Trim(), mode);

            await Groups.AddToGroupAsync(Context.ConnectionId, result.Game.Id);
            
            var response = new JoinGameResponseDto
            {
                GameId = result.Game.Id,
                PlayerId = result.Player.PlayerId,
                BoardSize = result.Game.Configuration.BoardSize,
                IsWaitingForOpponent = result.IsWaitingForOpponent,
                Message = result.IsWaitingForOpponent
                    ? $"Waiting for an opponent to join. Mode: {result.Game.Configuration.Mode}"
                    : $"Opponent joined. Mode: {result.Game.Configuration.Mode}. Prepare to place your fleet."
            };
            
            if (IsSinglePlayer)
            { 
                await _botQueue.EnqueueGameAsync(result.Game.Id);
            }
            
            if (result.IsWaitingForOpponent)
            {
                await Clients.Caller.SendAsync("WaitingForOpponent", new WaitingStatusDto
                {
                    GameId = result.Game.Id,
                    Message = $"Waiting for an opponent to join. Mode: {result.Game.Configuration.Mode}"
                });
            }
            else
            {
                await NotifyOpponentJoined(result.Game, result.Player);
                await NotifyPlacementPhase(result.Game);
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }
    }
    public async Task JoinGameAsBot(string gameId)
    {
        var result = _gameService.JoinGame(Context.ConnectionId, "Bot");
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        await Clients.Caller.SendAsync("BotConnectionAssigned", result.Player.PlayerId);
        await NotifyOpponentJoined(result.Game, result.Player);
        await NotifyPlacementPhase(result.Game);
        
    }

    public Task<IEnumerable<string>> GetAvailableGameModes()
    {
        return Task.FromResult(GameConfigurationPrototypes.GetAvailableModes());
    }

    public async Task<PlacementResultDto> PlaceShips(List<ShipPlacementDto> placements)
    {
        try
        {
            var placementsModel = placements.Select(MapPlacement).ToList();
            _gameService.ApplyPlacements(Context.ConnectionId, placementsModel);
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }

        var game = _gameService.GetGameByConnection(Context.ConnectionId) ?? throw new HubException("Game not found.");
        var player = game.FindPlayerByConnection(Context.ConnectionId) ?? throw new HubException("Player not found.");

        var placementResult = new PlacementResultDto
        {
            GameId = game.Id,
            AllPlayersReady = game.Players.All(p => p.IsReady),
            Message = "Fleet placement saved."
        };

        await Clients.Caller.SendAsync("PlacementConfirmed", placementResult);

        var opponent = game.GetOpponent(player.PlayerId);
        if (opponent is not null)
        {
            await Clients.Client(opponent.ConnectionId).SendAsync("OpponentReady", new OpponentStatusDto
            {
                GameId = game.Id,
                OpponentName = player.Name
            });
        }

        if (placementResult.AllPlayersReady && game.Phase == GamePhase.InProgress)
        {
            await NotifyGameStarted(game);
        }

        return placementResult;
    }

    public async Task<ShotResolutionDto> Fire(ShotRequestDto request)
    {
        FireResult fireResult;
        try
        {
            fireResult = _gameService.Fire(Context.ConnectionId, new Coordinate(request.Row, request.Column));
        }
        catch (Exception ex)
        {
            throw new HubException(ex.Message);
        }

        var game = _gameService.GetGameByConnection(Context.ConnectionId) ?? throw new HubException("Game not found.");
        var shotDto = new ShotResolutionDto
        {
            GameId = game.Id,
            ShooterId = fireResult.ShooterId,
            TargetPlayerId = fireResult.TargetPlayerId,
            Target = new CoordinateDto { Row = fireResult.Coordinate.Row, Column = fireResult.Coordinate.Column },
            Outcome = fireResult.Resolution.Outcome.ToString(),
            ShipName = fireResult.Resolution.Ship?.Type.ToString(),
            Victory = fireResult.IsVictory,
            NextPlayerId = fireResult.NextPlayerId
        };

        await Clients.Group(game.Id).SendAsync("ShotResolved", shotDto);

        if (fireResult.IsVictory)
        {
            var winner = game.FindPlayerById(fireResult.ShooterId);
            if (winner is not null)
            {
                await Clients.Group(game.Id).SendAsync("GameEnded", new GameEndedDto
                {
                    GameId = game.Id,
                    WinnerPlayerId = winner.PlayerId,
                    WinnerName = winner.Name
                });
            }
        }
        else if (!string.IsNullOrEmpty(fireResult.NextPlayerId))
        {
            await Clients.Group(game.Id).SendAsync("TurnChanged", new TurnChangedDto
            {
                GameId = game.Id,
                CurrentPlayerId = fireResult.NextPlayerId
            });
        }
 
        return shotDto;
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var game = _gameService.RemovePlayer(Context.ConnectionId);
        if (game is not null)
        {
            var opponent = game.Players.FirstOrDefault();
            if (opponent is not null)
            {
                await Clients.Client(opponent.ConnectionId).SendAsync("OpponentDisconnected", new WaitingStatusDto
                {
                    GameId = game.Id,
                    Message = "Opponent disconnected."
                });
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    private static ShipPlacement MapPlacement(ShipPlacementDto dto)
    {
        if (!Enum.TryParse<ShipOrientation>(dto.Orientation, true, out var orientation))
        {
            throw new InvalidOperationException("Invalid ship orientation.");
        }

        if (!Enum.TryParse<ShipType>(dto.ShipName, true, out var shipType))
        {
            throw new InvalidOperationException("Invalid ship type.");
        }

        return new ShipPlacement(shipType, new Coordinate(dto.Row, dto.Column), orientation);
    }

    private async Task NotifyOpponentJoined(Game game, GamePlayer joiningPlayer)
    {
        var opponent = game.GetOpponent(joiningPlayer.PlayerId);
        if (opponent is null)
        {
            return;
        }

        var dtoForOpponent = new OpponentStatusDto
        {
            GameId = game.Id,
            OpponentName = joiningPlayer.Name
        };

        var dtoForJoiningPlayer = new OpponentStatusDto
        {
            GameId = game.Id,
            OpponentName = opponent.Name
        };

        await Clients.Client(opponent.ConnectionId).SendAsync("OpponentJoined", dtoForOpponent);
        await Clients.Client(joiningPlayer.ConnectionId).SendAsync("OpponentJoined", dtoForJoiningPlayer);
    }

    private async Task NotifyPlacementPhase(Game game)
    {
        var fleet = _gameService.GetFleetDefinition()
            .Select(s => new ShipDefinitionDto { Name = s.Key.ToString(), Length = s.Value })
            .ToArray();

        var dto = new GameStartDto
        {
            GameId = game.Id,
            CurrentPlayerId = string.Empty,
            Fleet = fleet
        };

        await Clients.Group(game.Id).SendAsync("PlacementPhaseStarted", dto);
    }

    private async Task NotifyGameStarted(Game game)
    {
        var fleet = _gameService.GetFleetDefinition()
            .Select(s => new ShipDefinitionDto { Name = s.Key.ToString(), Length = s.Value })
            .ToArray();



        var dto = new GameStartDto
        {
            GameId = game.Id,
            CurrentPlayerId = game.CurrentTurnPlayerId ?? string.Empty,
            Fleet = fleet
        };

        await Clients.Group(game.Id).SendAsync("GameStarted", dto);
        if (!string.IsNullOrEmpty(dto.CurrentPlayerId))
        {
            await Clients.Group(game.Id).SendAsync("TurnChanged", new TurnChangedDto
            {
                GameId = game.Id,
                CurrentPlayerId = dto.CurrentPlayerId
            });
        }
    }
}
