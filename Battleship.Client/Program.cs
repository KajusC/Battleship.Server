using System;
using System.Windows.Forms;
using Battleship.Client.UI;

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
            Application.Run(new GameForm(connection.ServerUrl, connection.PlayerName));
        }
    }
}
