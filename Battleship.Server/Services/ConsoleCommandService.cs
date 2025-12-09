using Battleship.Server.Interpreter;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship.Server.Services
{
    public class ConsoleCommandService : BackgroundService
    {
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<ConsoleCommandService> _logger;

        public ConsoleCommandService(IHostApplicationLifetime lifetime, ILogger<ConsoleCommandService> logger)
        {
            _lifetime = lifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            
            await Task.Delay(1000, stoppingToken);

            _logger.LogInformation("Console Command Service Started. Type 'help' for commands.");
            Console.Write(new string('-', 10));
            Console.WriteLine('\n');

            var inputBuffer = new StringBuilder();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        var input = inputBuffer.ToString();
                        inputBuffer.Clear();
                        
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            _processCommand(input);
                        }
                        Console.Write("> ");
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (inputBuffer.Length > 0)
                        {
                            inputBuffer.Length--;
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        inputBuffer.Append(key.KeyChar);
                        Console.Write(key.KeyChar);
                    }
                }
                else
                {
                    await Task.Delay(50, stoppingToken);
                }
            }
        }

        private void _processCommand(string input)
        {
            try
            {
                var context = new InterpreterContext(input, _lifetime);
                var expression = CommandParser.Parse(input);
                expression.Interpret(context);
                if (!string.IsNullOrEmpty(context.Output))
                {
                    Console.WriteLine(context.Output);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing command: {ex.Message}");
            }
        }
    }
}