using System;

namespace Battleship.Server.Interpreter
{
    public class HelpExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  help          - Show this help message");
            Console.WriteLine("  status        - Show current game status");
            Console.WriteLine("  kick <player> - Kick a player by name");
            Console.WriteLine("  stop          - Stop the server");
        }
    }
}