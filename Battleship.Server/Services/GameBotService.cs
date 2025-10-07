using Battleship.Server.Models;
using Battleship.Server.Models.Dtos;
using Battleship.Server.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.Common;

public class GameBotService : BackgroundService
{
    private readonly BotCommandQueue _botQueue;
    private HubConnection? _connection;
    private NPC theBrainz;
    private GameService _gameservice;
    private string _botConnectionId;
    public GameBotService(GameService gameservice, BotCommandQueue botQueue)
    {
        _gameservice = gameservice;
        _botQueue = botQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var gameId in _botQueue.ReadAllAsync(stoppingToken))
        {
            //Console.WriteLine($"[BOT] Joining singleplayer game {gameId}");
            Game game = _gameservice.GetGameByConnection(gameId);
            theBrainz = new NPC(game);
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5260/gamehub")
                .Build();


            _connection.On<GameStartDto>("PlacementPhaseStarted", async result =>
            {                
                IReadOnlyDictionary<string, int> FleetDefinition = ShipSpecification.StandardFleet;
                List<ShipPlacementDto> placements = theBrainz.ShipPlacements(ShipSpecification.StandardFleet, 10, 10);
                
                await _connection.InvokeAsync("PlaceShips", placements, cancellationToken: stoppingToken);

            });
            _connection.On<TurnChangedDto>("TurnChanged", async result =>
            {
                //Console.WriteLine("{0} {1}", result.CurrentPlayerId, _botConnectionId);

                if (result.CurrentPlayerId == _botConnectionId)
                {
                    var rand = new Random();
                    //Console.WriteLine(" do {0} {1}", result.CurrentPlayerId, _botConnectionId);
                    Coordinate coord = theBrainz.DoStrategy();
                    var shotReq = new ShotRequestDto
                    {
                        GameId = result.GameId,
                        Row = coord.Row,
                        Column = coord.Column
                    };
                    //Console.WriteLine("coord {0} {1}", coord.Row, coord.Column);
                    theBrainz.lastRow = coord.Row;
                    theBrainz.lastColumn = coord.Column;
                    await _connection.InvokeAsync("Fire", shotReq, cancellationToken: stoppingToken);
                }
                // Maybe fire a first shot here
            });
            _connection.On<ShotResolutionDto>("ShotResolved", async result =>
            {
                //Console.WriteLine( "{0} : {1}", result.TargetPlayerId[0] , result.Outcome.ToString());
                if(result.TargetPlayerId != _botConnectionId)
                {
                    theBrainz.Update(Enum.Parse<ShotOutcome>(result.Outcome));                 
                }
            });
            _connection.On<string>("BotConnectionAssigned", id =>
            {
                //Console.WriteLine($"[BOT] My ConnectionId is {id}");
                _botConnectionId = id;
            });

            await _connection.StartAsync(stoppingToken);

            // Join the same group as the player
            await _connection.InvokeAsync("JoinGameAsBot", gameId, cancellationToken: stoppingToken);

            // Optionally do other actions, like placing ships or firing
        }
    }
}
