using Microsoft.AspNetCore.SignalR;

namespace Battleship.Server
{
    public class ChatHub : Hub
    {
        public ChatHub() { }

        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine("Received message, sending back echo");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
