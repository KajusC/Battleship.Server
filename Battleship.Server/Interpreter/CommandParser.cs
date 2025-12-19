using System;

namespace Battleship.Server.Interpreter
{
    public class CommandParser
    {
        public static IExpression Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return ExpressionFlyweightFactory.Get("unknown");

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return ExpressionFlyweightFactory.Get("unknown");

            var command = parts[0];
            return ExpressionFlyweightFactory.Get(command);
        }
    }
}
