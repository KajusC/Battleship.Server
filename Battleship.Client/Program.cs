using System;
using System.Windows.Forms;
using Battleship.Client.UI;
using Battleship.Client.Services; // Add this using directive for GameClient

namespace Battleship.Client;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var connection = new ConnectToServer();
        if (connection.ShowDialog() == DialogResult.OK &&
            !string.IsNullOrEmpty(connection.ServerUrl) &&
            !string.IsNullOrEmpty(connection.PlayerName))
        {
            Console.WriteLine($"Singleplayer: {connection.isSingleplayer}, Mode: {connection.GameMode}, useWeapons: {connection.useWeapons}");
            bool useWeapons=connection.useWeapons;
            //if (connection.isSingleplayer) useWeapons = false;
            Application.Run(new GameForm(connection.ServerUrl, connection.PlayerName, connection.isSingleplayer, connection.GameMode,useWeapons));
        }
    }
}
