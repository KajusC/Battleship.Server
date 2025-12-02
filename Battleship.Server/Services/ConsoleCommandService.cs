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

        public ConsoleCommandService(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Yield to let the application start up before printing prompt
            await Task.Yield();
            
            // Wait a bit more to ensure startup logs are done
            await Task.Delay(1000, stoppingToken);

            Console.WriteLine("\nConsole Command Service Started. Type 'help' for commands.");
            Console.Write("> ");

            var inputBuffer = new StringBuilder();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine(); // Move to next line
                        var input = inputBuffer.ToString();
                        inputBuffer.Clear();
                        
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            ProcessCommand(input);
                        }
                        Console.Write("> ");
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (inputBuffer.Length > 0)
                        {
                            inputBuffer.Length--;
                            Console.Write("\b \b"); // Erase character from console
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

        private void ProcessCommand(string input)
        {
            try
            {
                var context = new InterpreterContext(input, _lifetime);
                var expression = CommandParser.Parse(input);
                expression.Interpret(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing command: {ex.Message}");
            }
        }
    }
}