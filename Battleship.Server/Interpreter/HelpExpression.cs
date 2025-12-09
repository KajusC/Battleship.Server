using System;

namespace Battleship.Server.Interpreter
{
    public class HelpExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            context.Output = "Available commands:";
            context.Output += "\n  help          - Show this help message";
            context.Output += "\n  status        - Show current game status";
            context.Output += "\n  kick <player> - Kick a player by name";
            context.Output += "\n  stop          - Stop the server";
        }
    }
}