using Battleship.Server.Services;
using Microsoft.Extensions.Hosting;

namespace Battleship.Server.Interpreter
{
    public class InterpreterContext
    {
        public string[] InputArgs { get; }
        public IHostApplicationLifetime Lifetime { get; }
        public string Output { get; set; } = string.Empty;

        public InterpreterContext(string input, IHostApplicationLifetime lifetime)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            InputArgs = parts.Length > 0 ? parts : Array.Empty<string>();
            Lifetime = lifetime;
        }
    }
}