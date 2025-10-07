using Battleship.Server.Services;

namespace Battleship.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();
            builder.Services.AddSingleton<GameService>();
            builder.Services.AddHostedService<GameBotService>();
            builder.Services.AddSingleton<BotCommandQueue>();
            var app = builder.Build();

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
