using System;
using Battleship.Server.Services;

namespace Battleship.Server.Interpreter
{
    public class StatusExpression : IExpression
    {
        private readonly GameService _gameService = GameService.Instance;
        public void Interpret(InterpreterContext context)
        {
            context.Output = $"Active Games: {_gameService.ActiveGamesCount}\n";
            context.Output += $"Connected Players: {_gameService.ConnectedPlayersCount}\n";
            context.Output += $"All Users: {_gameService.GetAllUsers()}";
        }
    }
}