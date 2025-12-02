using Battleship.Server.Models;
using Battleship.Server.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Battleship.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<BotCommandQueue>();
            builder.Services.AddSingleton<IGameObserver, GameHubObserver>();
            builder.Services.AddHostedService<GameBotService>();
            builder.Services.AddHostedService<ConsoleCommandService>();
            var app = builder.Build();

            GameService.RegisterObservers(app.Services.GetServices<IGameObserver>());

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            app.MapHub<GameHub>("/gamehub");

            app.Run();
        }
    }
}
