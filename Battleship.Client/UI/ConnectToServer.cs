using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship.Client.UI
{
    public partial class ConnectToServer : Form
    {
        public string? ServerUrl { get; private set; }
        public string? PlayerName { get; private set; }

        public ConnectToServer()
        {
            InitializeComponent();
            
            // Set default player name
            textBoxPlayerName.Text = $"Commander-{Random.Shared.Next(1000, 9999)}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBoxServerUrl.Focus();
            textBoxServerUrl.SelectAll();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            textBoxServerUrl.Focus();
        }
    }
}
