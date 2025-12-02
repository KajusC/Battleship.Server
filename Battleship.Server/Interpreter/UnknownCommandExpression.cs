using System;

namespace Battleship.Server.Interpreter
{
    public class UnknownCommandExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            var cmd = context.Args.Length > 0 ? context.Args[0] : "";
            Console.WriteLine($"Unknown command: '{cmd}'. Type 'help' for available commands.");
        }
    }
}