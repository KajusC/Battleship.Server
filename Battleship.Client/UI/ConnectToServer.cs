using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship.Client.UI
{
    public partial class ConnectToServer : Form
    {
        public string? ServerUrl { get; private set; }
        public string? PlayerName { get; private set; }
        public bool isSingleplayer { get; private set; }


        public ConnectToServer()
        {

            
            InitializeComponent();

            // Set default player name
            textBoxPlayerName.Text = $"Commander-{Random.Shared.Next(1000, 9999)}";
            isSingleplayer = false;
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

        private void singleplayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (this.singleplayerCheck.Checked)
            {
                isSingleplayer=true;
                //this.BackColor = Color.Black;
                //this.singleplayerCheck.ForeColor = Color.White;
            }
            else
            {
                isSingleplayer = false;
                //this.BackColor = Color.White;
                //this.singleplayerCheck.ForeColor = Color.DarkGreen;
            }
        }
    }
}
