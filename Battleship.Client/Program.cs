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
        Application.Run(new GameForm());
    }
}
