using System;
using System.Collections.Concurrent;

namespace Battleship.Server.Interpreter
{
    public static class ExpressionFlyweightFactory
    {
        private static readonly ConcurrentDictionary<string, IExpression> Cache =
            new(StringComparer.OrdinalIgnoreCase);

        public static IExpression Get(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return Cache.GetOrAdd("unknown", _ => new UnknownCommandExpression());
            }

            var key = command.Trim().ToLowerInvariant();
            if (key == "exit") key = "stop";

            return Cache.GetOrAdd(key, Create);
        }

        private static IExpression Create(string key)
        {
            return key switch
            {
                "help" => new HelpExpression(),
                "status" => new StatusExpression(),
                "kick" => new KickExpression(),
                "stop" => new StopExpression(),
                "unknown" => new UnknownCommandExpression(),
                _ => new UnknownCommandExpression(),
            };
        }
    }
}
