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
            if (context.Args.Length < 2)
            {
                Console.WriteLine("Usage: kick <player name>");
                return;
            }

            var playerName = context.Args[1];
            var game = _gameService.RemovePlayerByName(playerName);
            
            if (game != null)
            {
                Console.WriteLine($"Player '{playerName}' kicked.");
            }
            else
            {
                Console.WriteLine($"Player '{playerName}' not found.");
            }
        }
    }
}
