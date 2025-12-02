using Battleship.Server.Services;
using Microsoft.Extensions.Hosting;

namespace Battleship.Server.Interpreter
{
    public class InterpreterContext
    {
        public string[] Args { get; }
        public IHostApplicationLifetime Lifetime { get; }

        public InterpreterContext(string input, IHostApplicationLifetime lifetime)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Args = parts.Length > 0 ? parts : Array.Empty<string>();
            Lifetime = lifetime;
        }
    }
}