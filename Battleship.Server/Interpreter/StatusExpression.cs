using System;
using Battleship.Server.Services;

namespace Battleship.Server.Interpreter
{
    public class StatusExpression : IExpression
    {
        private readonly GameService _gameService = GameService.Instance;
        public void Interpret(InterpreterContext context)
        {
            Console.WriteLine($"Active Games: {_gameService.ActiveGamesCount}");
            Console.WriteLine($"Connected Players: {_gameService.ConnectedPlayersCount}");
            Console.WriteLine($"All Users: {_gameService.GetAllUsers()}");
        }
    }
}