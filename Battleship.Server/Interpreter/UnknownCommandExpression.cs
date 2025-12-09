using System;

namespace Battleship.Server.Interpreter
{
    public class UnknownCommandExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            var cmd = context.InputArgs.Length > 0 ? context.InputArgs[0] : "";
            context.Output = $"Unknown command: '{cmd}'. Type 'help' for available commands.";
        }
    }
}