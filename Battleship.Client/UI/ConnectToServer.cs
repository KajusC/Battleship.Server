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
        public string GameMode { get; private set; } = "standard";
        public bool useWeapons { get; private set; } = false;


        public ConnectToServer()
        {

            
            InitializeComponent();

            // Set default player name
            textBoxPlayerName.Text = $"Commander-{Random.Shared.Next(1000, 9999)}";
            isSingleplayer = false;
            
            // Initialize game mode combo box
            InitializeGameModeComboBox();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBoxServerUrl.Focus();
            textBoxServerUrl.SelectAll();
        }

        private void InitializeGameModeComboBox()
        {
            // Add available game modes
            comboBoxGameMode.Items.AddRange(new object[]
            {
                "Standard (10x10, 5 ships, 60s)",
                "Quick (8x8, 3 ships, 30s)",
                "Epic (12x12, 9 ships, 90s)",
                "Massive (15x15, 14 ships, 120s)"
            });
            comboBoxGameMode.SelectedIndex = 0; // Default to Standard
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
        private void useWeaponsCheck_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine("check");
            if (this.useWeaponsCheck.Checked)
            {
                useWeapons = true;
                //this.BackColor = Color.Black;
                //this.singleplayerCheck.ForeColor = Color.White;
            }
            else
            {
                useWeapons = false;
                //this.BackColor = Color.White;
                //this.singleplayerCheck.ForeColor = Color.DarkGreen;
            }
        }
        private void comboBoxGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Map display text to game mode
            GameMode = comboBoxGameMode.SelectedIndex switch
            {
                0 => "standard",
                1 => "quick",
                2 => "Epic",
                3 => "Massive",
                _ => "standard"
            };
        }
    }
}
