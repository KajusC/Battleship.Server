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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxServerUrl = new System.Windows.Forms.TextBox();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.labelServerUrl = new System.Windows.Forms.Label();
            this.labelPlayerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.buttonConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonConnect.ForeColor = System.Drawing.Color.White;
            this.buttonConnect.Location = new System.Drawing.Point(150, 200);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(120, 40);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxServerUrl
            // 
            this.textBoxServerUrl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxServerUrl.Location = new System.Drawing.Point(50, 100);
            this.textBoxServerUrl.Name = "textBoxServerUrl";
            this.textBoxServerUrl.Size = new System.Drawing.Size(320, 25);
            this.textBoxServerUrl.TabIndex = 0;
            this.textBoxServerUrl.Text = "http://localhost:5000";
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxPlayerName.Location = new System.Drawing.Point(50, 160);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(320, 25);
            this.textBoxPlayerName.TabIndex = 1;
            // 
            // labelServerUrl
            // 
            this.labelServerUrl.AutoSize = true;
            this.labelServerUrl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelServerUrl.ForeColor = System.Drawing.Color.White;
            this.labelServerUrl.Location = new System.Drawing.Point(50, 80);
            this.labelServerUrl.Name = "labelServerUrl";
            this.labelServerUrl.Size = new System.Drawing.Size(136, 19);
            this.labelServerUrl.TabIndex = 3;
            this.labelServerUrl.Text = "Server IP Address:";
            // 
            // labelPlayerName
            // 
            this.labelPlayerName.AutoSize = true;
            this.labelPlayerName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelPlayerName.ForeColor = System.Drawing.Color.White;
            this.labelPlayerName.Location = new System.Drawing.Point(50, 140);
            this.labelPlayerName.Name = "labelPlayerName";
            this.labelPlayerName.Size = new System.Drawing.Size(91, 19);
            this.labelPlayerName.TabIndex = 4;
            this.labelPlayerName.Text = "Player Name:";
            // 
            // ConnectToServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(420, 280);
            this.Controls.Add(this.labelPlayerName);
            this.Controls.Add(this.labelServerUrl);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.textBoxServerUrl);
            this.Controls.Add(this.buttonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectToServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to Battleship Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxServerUrl;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Label labelServerUrl;
        private System.Windows.Forms.Label labelPlayerName;

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