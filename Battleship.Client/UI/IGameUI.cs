using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Battleship.Client.Models;
using Battleship.Client.Services;

namespace Battleship.Client.UI
{
    public interface IGameUI
    {
        void RenderBoard(BoardState playerBoard, BoardState opponentBoard);
        void DisplayMessage(string message);
        Task HandlePlayerActionAsync();
    }

    // Lightweight helper implementation. The real Windows Forms UI is `GameForm`
    // which should implement IGameUI directly. This helper won't perform interactive work.
    public sealed class WindowsGameUI : IGameUI
    {
        private readonly GameClient _client;

        public WindowsGameUI(GameClient client)
        {
            _client = client;
        }

        public void RenderBoard(BoardState playerBoard, BoardState opponentBoard)
        {
            // Intentionally minimal — GameForm handles actual rendering.
        }

        public void DisplayMessage(string message)
        {
            MessageBox.Show(message, "Battleship", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public Task HandlePlayerActionAsync()
        {
            // Helper implementation does not await UI actions.
            return Task.CompletedTask;
        }
    }
}
