using System;
using Battleship.Server.Models;
using Battleship.Server.Services;

namespace Battleship.Server.Interpreter
{
    public class KickExpression : IExpression
    {
        private readonly GameService _gameService = GameService.Instance;
        public void Interpret(InterpreterContext context)
        {
            if (context.InputArgs.Length < 2)
            {
                context.Output = "Usage: kick <player name>";
                return;
            }

            var playerName = context.InputArgs[1];
            var game = _gameService.RemovePlayerByName(playerName);
            
            if (game != null)
            {
                context.Output = $"Player '{playerName}' kicked.";
            }
            else
            {
                context.Output = $"Player '{playerName}' not found.";
            }
        }
    }
}
