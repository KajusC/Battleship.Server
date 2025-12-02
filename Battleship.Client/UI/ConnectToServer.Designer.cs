namespace Battleship.Client.UI
{
    partial class ConnectToServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonConnect = new Button();
            textBoxServerUrl = new TextBox();
            textBoxPlayerName = new TextBox();
            labelServerUrl = new Label();
            labelPlayerName = new Label();
            singleplayerCheck = new CheckBox();
            useWeaponsCheck = new CheckBox();
            comboBoxGameMode = new ComboBox();
            labelGameMode = new Label();
            SuspendLayout();
            // 
            // buttonConnect
            // 
            buttonConnect.BackColor = Color.FromArgb(70, 130, 180);
            buttonConnect.FlatStyle = FlatStyle.Flat;
            buttonConnect.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            buttonConnect.ForeColor = Color.White;
            buttonConnect.Location = new Point(171, 370);
            buttonConnect.Margin = new Padding(3, 4, 3, 4);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(137, 53);
            buttonConnect.TabIndex = 3;
            buttonConnect.Text = "Connect";
            buttonConnect.UseVisualStyleBackColor = false;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // textBoxServerUrl
            // 
            textBoxServerUrl.Font = new Font("Segoe UI", 10F);
            textBoxServerUrl.Location = new Point(57, 133);
            textBoxServerUrl.Margin = new Padding(3, 4, 3, 4);
            textBoxServerUrl.Name = "textBoxServerUrl";
            textBoxServerUrl.Size = new Size(365, 30);
            textBoxServerUrl.TabIndex = 0;
            textBoxServerUrl.Text = "http://localhost:5260";
            // 
            // textBoxPlayerName
            // 
            textBoxPlayerName.Font = new Font("Segoe UI", 10F);
            textBoxPlayerName.Location = new Point(57, 213);
            textBoxPlayerName.Margin = new Padding(3, 4, 3, 4);
            textBoxPlayerName.Name = "textBoxPlayerName";
            textBoxPlayerName.Size = new Size(365, 30);
            textBoxPlayerName.TabIndex = 1;
            // 
            // labelServerUrl
            // 
            labelServerUrl.AutoSize = true;
            labelServerUrl.Font = new Font("Segoe UI", 10F);
            labelServerUrl.ForeColor = Color.White;
            labelServerUrl.Location = new Point(57, 107);
            labelServerUrl.Name = "labelServerUrl";
            labelServerUrl.Size = new Size(146, 23);
            labelServerUrl.TabIndex = 3;
            labelServerUrl.Text = "Server IP Address:";
            // 
            // labelPlayerName
            // 
            labelPlayerName.AutoSize = true;
            labelPlayerName.Font = new Font("Segoe UI", 10F);
            labelPlayerName.ForeColor = Color.White;
            labelPlayerName.Location = new Point(57, 187);
            labelPlayerName.Name = "labelPlayerName";
            labelPlayerName.Size = new Size(111, 23);
            labelPlayerName.TabIndex = 4;
            labelPlayerName.Text = "Player Name:";
            // 
            // singleplayerCheck
            // 
            singleplayerCheck.BackColor = Color.LightGray;
            singleplayerCheck.FlatStyle = FlatStyle.Flat;
            singleplayerCheck.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            singleplayerCheck.ForeColor = Color.DarkGreen;
            singleplayerCheck.Location = new Point(50, 20);
            singleplayerCheck.Margin = new Padding(3, 4, 3, 4);
            singleplayerCheck.Name = "singleplayerCheck";
            singleplayerCheck.Padding = new Padding(6, 7, 6, 7);
            singleplayerCheck.Size = new Size(150, 50);
            singleplayerCheck.TabIndex = 5;
            singleplayerCheck.Text = "Singleplayer Mode";
            singleplayerCheck.UseVisualStyleBackColor = false;
            singleplayerCheck.CheckedChanged += singleplayerCheck_CheckedChanged;
            // 
            // useWeaponsCheck
            // 
            useWeaponsCheck.BackColor = Color.LightGray;
            useWeaponsCheck.FlatStyle = FlatStyle.Flat;
            useWeaponsCheck.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            useWeaponsCheck.ForeColor = Color.DarkGreen;
            useWeaponsCheck.Location = new Point(250, 20);
            useWeaponsCheck.Margin = new Padding(3, 4, 3, 4);
            useWeaponsCheck.Name = "useWeaponsCheck";
            useWeaponsCheck.Padding = new Padding(6, 7, 6, 7);
            useWeaponsCheck.Size = new Size(150, 50);
            useWeaponsCheck.TabIndex = 5;
            useWeaponsCheck.Text = "Use Weapons";
            useWeaponsCheck.UseVisualStyleBackColor = false;
            useWeaponsCheck.CheckedChanged += useWeaponsCheck_CheckedChanged;
            // 
            // comboBoxGameMode
            // 
            comboBoxGameMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxGameMode.Font = new Font("Segoe UI", 10F);
            comboBoxGameMode.Location = new Point(57, 293);
            comboBoxGameMode.Margin = new Padding(3, 4, 3, 4);
            comboBoxGameMode.Name = "comboBoxGameMode";
            comboBoxGameMode.Size = new Size(365, 31);
            comboBoxGameMode.TabIndex = 2;
            comboBoxGameMode.SelectedIndexChanged += comboBoxGameMode_SelectedIndexChanged;
            // 
            // labelGameMode
            // 
            labelGameMode.AutoSize = true;
            labelGameMode.Font = new Font("Segoe UI", 10F);
            labelGameMode.ForeColor = Color.White;
            labelGameMode.Location = new Point(57, 267);
            labelGameMode.Name = "labelGameMode";
            labelGameMode.Size = new Size(102, 23);
            labelGameMode.TabIndex = 6;
            labelGameMode.Text = "Game Mode:";
            // 
            // ConnectToServer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(35, 40, 55);
            ClientSize = new Size(480, 473);
            Controls.Add(labelGameMode);
            Controls.Add(comboBoxGameMode);
            Controls.Add(labelPlayerName);
            Controls.Add(labelServerUrl);
            Controls.Add(textBoxPlayerName);
            Controls.Add(textBoxServerUrl);
            Controls.Add(buttonConnect);
            Controls.Add(singleplayerCheck);
            Controls.Add(useWeaponsCheck);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectToServer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Connect to Battleship Server";
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxServerUrl;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Label labelServerUrl;
        private System.Windows.Forms.Label labelPlayerName;
        private System.Windows.Forms.CheckBox singleplayerCheck;
        private System.Windows.Forms.CheckBox useWeaponsCheck;

        private System.Windows.Forms.ComboBox comboBoxGameMode;
        private System.Windows.Forms.Label labelGameMode;

        private async void buttonConnect_Click(object sender, System.EventArgs e)
        {
            await ConnectButtonClickAsync();
        }
        

        private async Task ConnectButtonClickAsync()
        {
            if (string.IsNullOrWhiteSpace(textBoxServerUrl.Text))
            {
                MessageBox.Show("Please enter a server IP address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxServerUrl.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPlayerName.Text))
            {
                MessageBox.Show("Please enter your player name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBoxPlayerName.Focus();
                return;
            }

            ServerUrl = textBoxServerUrl.Text.Trim();
            PlayerName = textBoxPlayerName.Text.Trim();

            buttonConnect.Enabled = false;
            buttonConnect.Text = "Connecting...";

            try
            {
                // Validate URL format
                if (!ServerUrl.StartsWith("http://") && !ServerUrl.StartsWith("https://"))
                {
                    ServerUrl = "http://" + ServerUrl;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid server address: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                buttonConnect.Text = "Connect";
                buttonConnect.Enabled = true;
            }
        }
    }
}