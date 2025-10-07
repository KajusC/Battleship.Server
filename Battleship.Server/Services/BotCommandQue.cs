using System.Threading.Channels;

namespace Battleship.Server.Services
{
    public class BotCommandQueue
    {
        private readonly Channel<string> _channel = Channel.CreateUnbounded<string>();

        public async Task EnqueueGameAsync(string gameId)
            => await _channel.Writer.WriteAsync(gameId);

        public IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken)
            => _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
