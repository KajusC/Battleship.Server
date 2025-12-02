using System;

namespace Battleship.Server.Interpreter
{
    public class CommandParser
    {
        public static IExpression Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new UnknownCommandExpression();

            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return new UnknownCommandExpression();

            var command = parts[0].ToLower();

            return command switch
            {
                "help" => new HelpExpression(),
                "status" => new StatusExpression(),
                "kick" => new KickExpression(),
                "stop" => new StopExpression(),
                "exit" => new StopExpression(),
                _ => new UnknownCommandExpression()
            };
        }
    }
}
