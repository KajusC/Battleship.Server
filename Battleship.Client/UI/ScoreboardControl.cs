using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Battleship.Client.UI;

public class ScoreboardControl : UserControl
{
    private Panel player1Panel = null!;
    private Panel player2Panel = null!;
    private Label titleLabel = null!;
    private Label gamePhaseLabelTitle = null!;
    private Label gamePhaseLabel = null!;
    
    private Label p1NameLabel = null!;
    private Label p1ShipsLabel = null!;
    private Label p1ShotsLabel = null!;
    private Label p1AccuracyLabel = null!;
    private Label p1ScoreLabel = null!;
    private Panel p1TurnIndicator = null!;
    
    private Label p2NameLabel = null!;
    private Label p2ShipsLabel = null!;
    private Label p2ShotsLabel = null!;
    private Label p2AccuracyLabel = null!;
    private Label p2ScoreLabel = null!;
    private Panel p2TurnIndicator = null!;

    public ScoreboardControl()
    {
        InitializeComponents();
        SetupLayout();
    }

    private void InitializeComponents()
    {
        this.BackColor = Color.FromArgb(20, 30, 48);
        this.Size = new Size(600, 300);
        this.Padding = new Padding(10);

        titleLabel = new Label
        {
            Text = "SCOREBOARD",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top,
            Height = 40
        };

        // Game Phase
        gamePhaseLabelTitle = new Label
        {
            Text = "Game Phase:",
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.LightGray,
            AutoSize = true,
            Location = new Point(10, 45)
        };

        gamePhaseLabel = new Label
        {
            Text = "Waiting",
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.Yellow,
            AutoSize = true,
            Location = new Point(110, 45)
        };

        player1Panel = CreatePlayerPanel("Player 1", 70);
        player2Panel = CreatePlayerPanel("Player 2", 185);

        // Player 1 UI elements (positions relative to panel)
        p1TurnIndicator = new Panel { Size = new Size(20, 20), Location = new Point(10, 10), BackColor = Color.Gray, BorderStyle = BorderStyle.FixedSingle, Visible = false };
        p1NameLabel = CreateLabel("Admiral Smith", 40, 10, true, 18);
        p1ShipsLabel = CreateLabel("Ships: 5/5", 40, 35);
        p1ShotsLabel = CreateLabel("Shots: 0 (0 hits, 0 misses)", 40, 55);
        p1AccuracyLabel = CreateLabel("Accuracy: 0.0%", 40, 75);
        p1ScoreLabel = CreateLabel("Score: 0", 400, 35, false, 12, FontStyle.Bold);


        // Player 2 UI elements (positions relative to panel)
        p2TurnIndicator = new Panel { Size = new Size(20, 20), Location = new Point(10, 10), BackColor = Color.Gray, BorderStyle = BorderStyle.FixedSingle, Visible = false };
        p2NameLabel = CreateLabel("Captain Jones", 40, 10, true, 18);
        p2ShipsLabel = CreateLabel("Ships: 5/5", 40, 35);
        p2ShotsLabel = CreateLabel("Shots: 0 (0 hits, 0 misses)", 40, 55);
        p2AccuracyLabel = CreateLabel("Accuracy: 0.0%", 40, 75);
        p2ScoreLabel = CreateLabel("Score: 0", 400, 35, false, 12, FontStyle.Bold);

        this.Controls.Add(titleLabel);
        this.Controls.Add(gamePhaseLabelTitle);
        this.Controls.Add(gamePhaseLabel);
        // Add child controls to panels, then panels to control
        player1Panel.Controls.Add(p1TurnIndicator);
        player1Panel.Controls.Add(p1NameLabel);
        player1Panel.Controls.Add(p1ShipsLabel);
        player1Panel.Controls.Add(p1ShotsLabel);
        player1Panel.Controls.Add(p1AccuracyLabel);
        player1Panel.Controls.Add(p1ScoreLabel);

        player2Panel.Controls.Add(p2TurnIndicator);
        player2Panel.Controls.Add(p2NameLabel);
        player2Panel.Controls.Add(p2ShipsLabel);
        player2Panel.Controls.Add(p2ShotsLabel);
        player2Panel.Controls.Add(p2AccuracyLabel);
        player2Panel.Controls.Add(p2ScoreLabel);

        this.Controls.Add(player1Panel);
        this.Controls.Add(player2Panel);
    }

    private Panel CreatePlayerPanel(string title, int y)
    {
        var panel = new Panel
        {
            Location = new Point(10, y),
            Size = new Size(this.Width - 20, 105),
            BackColor = Color.FromArgb(30, 40, 60),
            BorderStyle = BorderStyle.FixedSingle
        };
        return panel;
    }

    private Panel CreateTurnIndicator(int x, int y)
    {
        var indicator = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(20, 20),
            BackColor = Color.Gray,
            BorderStyle = BorderStyle.FixedSingle,
            Visible = false
        };
        return indicator;
    }

    private Label CreateLabel(string text, int x, int y, bool bold = false, int fontSize = 10, FontStyle style = FontStyle.Regular)
    {
        return new Label
        {
            Text = text,
            Location = new Point(x, y),
            AutoSize = true,
            Font = new Font("Segoe UI", fontSize, bold ? FontStyle.Bold : style),
            ForeColor = Color.White,
            BackColor = Color.Transparent
        };
    }

    private void SetupLayout()
    {
        this.Resize += (s, e) =>
        {
            player1Panel.Width = this.Width - 20;
            player2Panel.Width = this.Width - 20;
        };
    }

    public void UpdateScoreboard(Battleship.Client.Networking.ScoreboardMessage scoreboardData)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => UpdateScoreboard(scoreboardData)));
            return;
        }

        try
        {
            Console.WriteLine("📊 ScoreboardControl: Received update");
            
            var phase = scoreboardData.GamePhase ?? "Unknown";
            gamePhaseLabel.Text = phase;
            gamePhaseLabel.ForeColor = GetPhaseColor(phase);
            
            Console.WriteLine($"   Game Phase: {gamePhaseLabel.Text}");

            var players = scoreboardData.PlayerScores?.ToList() ?? new System.Collections.Generic.List<Battleship.Client.Networking.PlayerScoreMessage>();
            if (players.Count == 0)
            {
                Console.WriteLine("   ⚠️ No players in scoreboard!");
                return;
            }

            Console.WriteLine($"   Player count: {players.Count}");
            
            // Update first player (if present)
            var p1 = players.ElementAtOrDefault(0);
            if (p1 != null) UpdatePlayerDisplay(p1, p1NameLabel, p1ShipsLabel, p1ShotsLabel, p1AccuracyLabel, p1ScoreLabel, p1TurnIndicator);
            
            // Update second player (if present)
            var p2 = players.ElementAtOrDefault(1);
            if (p2 != null) UpdatePlayerDisplay(p2, p2NameLabel, p2ShipsLabel, p2ShotsLabel, p2AccuracyLabel, p2ScoreLabel, p2TurnIndicator);
            
            Console.WriteLine("   ✅ Scoreboard updated successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error updating scoreboard: {ex.Message}");
            Console.WriteLine($"   Stack: {ex.StackTrace}");
        }
    }

    private void UpdatePlayerDisplay(Battleship.Client.Networking.PlayerScoreMessage playerScore, Label nameLabel, Label shipsLabel, 
        Label shotsLabel, Label accuracyLabel, Label scoreLabel, Panel turnIndicator)
    {
        nameLabel.Text = playerScore.PlayerName ?? "Unknown";
        shipsLabel.Text = $"Ships: {playerScore.ShipsRemaining}/{playerScore.TotalShips} remaining";
        shotsLabel.Text = $"Shots: {playerScore.TotalShotsFired} ({playerScore.TotalHits} hits, {playerScore.TotalMisses} misses)";
        
        var accuracy = playerScore.Accuracy;
        accuracyLabel.Text = $"Accuracy: {accuracy:F1}%";
        accuracyLabel.ForeColor = GetAccuracyColor(accuracy);
        
        scoreLabel.Text = $"Score: {playerScore.CalculatedScore}";
        
        var isCurrentTurn = playerScore.IsCurrentTurn;
        turnIndicator.Visible = isCurrentTurn;
        turnIndicator.BackColor = isCurrentTurn ? Color.LimeGreen : Color.Gray;
    }

    private Color GetPhaseColor(string phase)
    {
        return phase switch
        {
            "WaitingForPlayers" => Color.Gray,
            "Placement" => Color.Yellow,
            "InProgress" => Color.LimeGreen,
            "Completed" => Color.Red,
            _ => Color.White
        };
    }

    private Color GetAccuracyColor(double accuracy)
    {
        if (accuracy >= 70) return Color.LimeGreen;
        if (accuracy >= 50) return Color.Yellow;
        if (accuracy >= 30) return Color.Orange;
        return Color.Red;
    }

    public void Reset()
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(Reset));
            return;
        }

        gamePhaseLabel.Text = "Waiting";
        gamePhaseLabel.ForeColor = Color.Gray;

        ResetPlayerDisplay(p1NameLabel, p1ShipsLabel, p1ShotsLabel, p1AccuracyLabel, p1ScoreLabel, p1TurnIndicator);
        ResetPlayerDisplay(p2NameLabel, p2ShipsLabel, p2ShotsLabel, p2AccuracyLabel, p2ScoreLabel, p2TurnIndicator);
    }

    private void ResetPlayerDisplay(Label nameLabel, Label shipsLabel, Label shotsLabel, 
        Label accuracyLabel, Label scoreLabel, Panel turnIndicator)
    {
        nameLabel.Text = "Waiting for player...";
        shipsLabel.Text = "Ships: 0/0";
        shotsLabel.Text = "Shots: 0 (0 hits, 0 misses)";
        accuracyLabel.Text = "Accuracy: 0.0%";
        accuracyLabel.ForeColor = Color.White;
        scoreLabel.Text = "Score: 0";
        turnIndicator.Visible = false;
    }
}
