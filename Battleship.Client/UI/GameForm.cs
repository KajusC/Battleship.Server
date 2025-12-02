using Battleship.Client.Audio;
using Battleship.Client.Models;
using Battleship.Client.Models.Command;
using Battleship.Client.Networking;
using Battleship.Client.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship.Client.UI;

public sealed class GameForm : Form
{
    private readonly GameClient _client;
    private readonly BoardControl _playerBoardControl;
    private readonly BoardControl _opponentBoardControl;
    private readonly LogControl _logControl;
    private readonly ShipQueueControl _shipQueueControl;
    private readonly ActionButton _rotateButton;
    private readonly ActionButton _resetButton;
    private readonly ScoreboardControl _scoreboardControl;

    private readonly ActionButton _Weapon0Button;
    private readonly ActionButton _Weapon1Button;
    private readonly ActionButton _Weapon2Button;
    private readonly ActionButton _Weapon3Button;
    //Command
    private readonly ActionButton _placeButton;
    private readonly ActionButton _rotateOneButton;
    private readonly ActionButton _moveButton;
    private readonly ActionButton _backButton;
    private readonly ActionButton _submitPlacementsButton;
    //Command
    private PlacementController placementsController = new();
    private ShipPlacement? tempPl = null;
    private string commandType = "Place";
    private ShipPlacements placemenetsReceiver;

    private string serverIp;
    private string _gameMode;

    private BoardState _playerBoardState = new(10);
    private BoardState _opponentBoardState = new(10);

    private    IMusic? _currentMusic;

    private readonly List<ShipDefinition> _fleet = new();
    private readonly List<ShipPlacement> _placements = new();
    private ShipDefinition? _currentShip;
    private ShipOrientation _currentOrientation = ShipOrientation.Horizontal;
    private bool _isMyTurn;
    private string _playerName;
    private string? _opponentName;
    private bool isSinglePlayer;
    private int WeaponType = 0;
    private GameStage _stage = GameStage.Connecting;

    private GameState gameState = new StartingState();
    private int playerPoints = 0;
    private int opponentPoints = 0;
    public GameForm(string serverIp, string playerName, bool isSinglePlayer, string gameMode = "standard", bool useWeapons = false)
    {
        this.serverIp = serverIp;
        this._playerName = playerName;
        this.isSinglePlayer = isSinglePlayer;
        this._gameMode = gameMode;
        
        Text = isSinglePlayer ? $"Battleship Singleplayer - {gameMode}" : $"Battleship Multiplayer - {gameMode}";
        BackColor = Color.FromArgb(35, 40, 55);
        ForeColor = Color.WhiteSmoke;
        
        // Start with standard size, will be adjusted when board size is received
        ClientSize = new Size(1100, 680);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        DoubleBuffered = true;
        KeyPreview = true;
        
        //Command
        placemenetsReceiver = new(_placements);


        PlayMusic(MusicType.Menu);
        _playerBoardControl = new BoardControl
        {
            Location = new Point(40, 140),
            ShowShips = true
        };
        _playerBoardControl.CellClicked += PlayerBoardOnCellClicked;

        _opponentBoardControl = new BoardControl
        {
            Location = new Point(420, 140),
            ShowShips = false
        };
        _opponentBoardControl.CellClicked += OpponentBoardOnCellClicked;

        _logControl = new LogControl
        {
            Location = new Point(40, 430),
            Size = new Size(760, 210)
        };

        _shipQueueControl = new ShipQueueControl
        {
            Location = new Point(830, 140),
            Size = new Size(230, 250)
        };

        _rotateButton = new ActionButton
        {
            Text = "Rotate (R)",
            Location = new Point(830, 410)
        };
        _rotateButton.Click += (_, _) => ToggleOrientation();

        _resetButton = new ActionButton
        {
            Text = "Reset placement",
            Location = new Point(830, 460)
        };
        _resetButton.Click += (_, _) => ResetPlacements();

        _scoreboardControl = new ScoreboardControl
        {
            Location = new Point(40, 660),
            Size = new Size(760, 250)
        };
        _Weapon0Button = new ActionButton
        {
            Text = "Weapon: Cannon (1x1)",
            Location = new Point(500, 0)
        };
        _Weapon0Button.Click += (_, _) => SetWeapon(0);
        _Weapon1Button = new ActionButton
        {
            Text = "Weapon: Rocket (Cross)",
            Location = new Point(650, 0)
        };
        _Weapon1Button.Click += (_, _) => SetWeapon(1);
        _Weapon2Button = new ActionButton
        {
            Text = "Weapon: Bomb (3x3)",
            Location = new Point(800, 0)
        };
        _Weapon2Button.Click += (_, _) => SetWeapon(2);

        _Weapon3Button = new ActionButton
        {
            Text = "Weapon: Misile swarm (1x1, free turn)",
            Location = new Point(950, 0)
        };
        _Weapon3Button.Click += (_, _) => SetWeapon(3);
        _Weapon0Button.Visible = useWeapons;
        _Weapon1Button.Visible = useWeapons;
        _Weapon2Button.Visible = useWeapons;
        _Weapon3Button.Visible = useWeapons;

        _rotateButton = new ActionButton
        {
            Text = "Rotate (R)",
            Location = new Point(830, 410)
        };
        _rotateButton.Click += (_, _) => ToggleOrientation();
        _rotateButton = new ActionButton
        {
            Text = "Rotate (R)",
            Location = new Point(830, 410)
        };
        _rotateButton.Click += (_, _) => ToggleOrientation();

        _resetButton = new ActionButton
        {
            Text = "Reset placement",
            Location = new Point(830, 460)
        };
        _resetButton.Click += (_, _) => ResetPlacements();

        _placeButton = new ActionButton
        {
            Text = "C:Place",
            Location = new Point(1000, 410)
        };
        _placeButton.Click += (_, _) => Place();

        _rotateOneButton = new ActionButton
        {
            Text = "C:Rotate",
            Location = new Point(1000, 460)
        };
        _rotateOneButton.Click += (_, _) => Rotate();

        _moveButton = new ActionButton
        {
            Text = "C:move",
            Location = new Point(1000, 510)
        };
        _moveButton.Click += (_, _) => Move();
        _backButton = new ActionButton
        {
            Text = "C:Back",
            Location = new Point(1000, 560)
        };
        _backButton.Click += (_, _) => UndoCommand();
        _submitPlacementsButton = new ActionButton
        {
            Text = "C:Submit",
            Location = new Point(830, 560)
        };
        _submitPlacementsButton.Click += (_, _) => SubmitPlacements();


        Controls.AddRange(new Control[]
        {
            _playerBoardControl,
            _opponentBoardControl,
            _logControl,
            _shipQueueControl,
            _rotateButton,
            _resetButton,
            _scoreboardControl,
            //Template
            _Weapon0Button,
            _Weapon1Button,
            _Weapon2Button,
            _Weapon3Button,
            //Command           
            _rotateButton,
            _resetButton,
            _rotateButton,
            _placeButton,
            _rotateOneButton,
            _moveButton,
            _backButton,
            _submitPlacementsButton
            //
        });

        _client = new GameClient($"{serverIp}/gamehub");
        RegisterClientEvents();

        Shown += async (_, _) => await InitializeAsync();
        FormClosing += async (_, _) => await _client.DisposeAsync();

        KeyDown += GameForm_KeyDown;
    }

    private void PlayMusic(MusicType type)
    {
        _currentMusic?.Stop();
        _currentMusic = MusicFactory.CreateMusic(type);
        _currentMusic.Play();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        using var titleFont = new Font("Segoe UI", 20f, FontStyle.Bold, GraphicsUnit.Point);
        using var infoFont = new Font("Segoe UI", 10f, FontStyle.Regular, GraphicsUnit.Point);
        using var statusFont = new Font("Segoe UI", 12f, FontStyle.Bold, GraphicsUnit.Point);
        if (isSinglePlayer) g.DrawString("Battleship SinglePlayer", titleFont, Brushes.WhiteSmoke, new PointF(40, 40));
        else g.DrawString("Battleship Multiplayer", titleFont, Brushes.WhiteSmoke, new PointF(40, 40));
        g.DrawString($"Player: {_playerName}", infoFont, Brushes.WhiteSmoke, new PointF(40, 90));
        g.DrawString($"Stage: {_stage}", infoFont, Brushes.WhiteSmoke, new PointF(40, 110));

        if (!string.IsNullOrWhiteSpace(_opponentName))
        {
            g.DrawString($"Opponent: {_opponentName}", infoFont, Brushes.WhiteSmoke, new PointF(420, 110));
        }

        if (_stage == GameStage.InProgress)
        {
            var turnMessage = _isMyTurn ? "Your turn" : "Opponent's turn";
            g.DrawString(turnMessage, statusFont, _isMyTurn ? Brushes.LightGreen : Brushes.Orange, new PointF(420, 80));
        }

        g.DrawString("Your fleet", infoFont, Brushes.WhiteSmoke, new PointF(830, 120));
    }

    private async Task InitializeAsync()
    {
        AddLog("Connecting to server...");
        try
        {
            await _client.ConnectAsync();
            AddLog($"Connected. Joining game lobby (Mode: {_gameMode})...");

            var joinResponse = await _client.JoinGameAsync(_playerName, isSinglePlayer, _gameMode);
            ConfigureBoardSizes(joinResponse.BoardSize);
            AddLog(joinResponse.Message);

            if (joinResponse.IsWaitingForOpponent)
            {
                _stage = GameStage.WaitingForOpponent;
            }
            else
            {
                _stage = GameStage.Placement;
            }
        }
        catch (Exception ex)
        {
            _stage = GameStage.Completed;
            AddLog($"Connection error: {ex.Message}");
            MessageBox.Show($"Failed to connect: {ex.Message}", "Battleship", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Invalidate();
        }
    }

    private void RegisterClientEvents()
    {
        _client.WaitingForOpponent += message => SafeInvoke(() =>
        {
            _stage = GameStage.WaitingForOpponent;
            AddLog(message.Message);
            Invalidate();
        });

        _client.GamePhaseChanged += message => SafeInvoke(() =>
        {
            _stage = MapPhaseToStage(message.NewPhase);
            Invalidate();
        });

        _client.OpponentJoined += message => SafeInvoke(() =>
        {
            _opponentName = message.OpponentName;
            AddLog($"Opponent joined: {message.OpponentName}");
            Invalidate();
        });

        _client.PlacementPhaseStarted += message => SafeInvoke(() =>
        {
            StartPlacementPhase(message);
        });

        _client.OpponentReady += message => SafeInvoke(() =>
        {
            AddLog($"Opponent is ready: {message.OpponentName}");
        });

        _client.PlacementConfirmed += message => SafeInvoke(() =>
        {
            AddLog(message.Message);
            if (!message.AllPlayersReady)
            {
                AddLog("Waiting for opponent to finish placements...");
            }
        });

        _client.GameStarted += message => SafeInvoke(() =>
        {
            _stage = GameStage.InProgress;
            SetGameState(new WinningState());
            _isMyTurn = string.Equals(message.CurrentPlayerId, _client.PlayerId, StringComparison.Ordinal);
            AddLog(_isMyTurn ? "You will start the game." : "Opponent will start the game.");
            Invalidate();
        });

        _client.ShotResolved += message => SafeInvoke(() =>
        {
            ApplyShot(message);
        });

        _client.TurnChanged += message => SafeInvoke(() =>
        {
            _isMyTurn = string.Equals(message.CurrentPlayerId, _client.PlayerId, StringComparison.Ordinal);
            Invalidate();
        });

        _client.GameEnded += message => SafeInvoke(() =>
        {
            _stage = GameStage.Completed;
            var isWinner = string.Equals(message.WinnerPlayerId, _client.PlayerId, StringComparison.Ordinal);
            AddLog(isWinner ? "You won the battle!" : $"{message.WinnerName} won the battle.");
            SetGameState(isWinner ? new WonState() : new LostState());
            //PlayMusic(isWinner ? MusicType.Victory : MusicType.Defeat);
            MessageBox.Show(isWinner ? "Victory!" : "Defeat...", "Game finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Invalidate();
        });

        _client.OpponentDisconnected += message => SafeInvoke(() =>
        {
            _stage = GameStage.WaitingForOpponent;
            _opponentName = null;
            _isMyTurn = false;
            AddLog(message.Message);
            Invalidate();
        });

        _client.ErrorReceived += message => SafeInvoke(() =>
        {
            AddLog($"Server error: {message.Message}");
        });

        _client.ScoreboardUpdated += scoreboardData => SafeInvoke(() =>
        {
            Console.WriteLine("🎮 GameForm: Received ScoreboardUpdated event");
            _scoreboardControl.UpdateScoreboard(scoreboardData);
        });
    }

    private void StartPlacementPhase(GameStartMessage message)
    {
        _stage = GameStage.Placement;
        _fleet.Clear();
        _fleet.AddRange(message.Fleet.Select(f => new ShipDefinition(f.Name, f.Length)));
        _shipQueueControl.SetFleet(_fleet);
        _placements.Clear();
        _playerBoardState.Clear(CellState.Unknown);
        _playerBoardControl.Board = _playerBoardState;
        _opponentBoardControl.Board = _opponentBoardState;
        AdvancePlacementQueue();
        AddLog("Place your ships on the left board. Left click to place, right click to rotate, or press R to rotate.");
        Invalidate();
    }

    private void AdvancePlacementQueue()
    {
        _currentShip = _fleet.FirstOrDefault(ship => _placements.All(p => !string.Equals(p.Name, ship.Name, StringComparison.OrdinalIgnoreCase)));
        _shipQueueControl.SetCurrentShip(_currentShip?.Name);
        Invalidate();
    }

    private void PlayerBoardOnCellClicked(object? sender, BoardCellEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            ToggleOrientation();
            return;
        }

        if (_stage != GameStage.Placement)
        {
            return;
        }
        switch (commandType)
        {
            case "Place":
                if (_currentShip is null)
                {
                    AddLog($"All fleet is in place.");
                    return;
                }
                if (!CanPlaceShip(e.Row, e.Column, _currentShip.Length, _currentOrientation))
                {
                    SystemSounds.Beep.Play();
                    AddLog($"Cannot place {_currentShip.Name} at {(char)('A' + e.Column)}{e.Row + 1}.");
                    return;
                }
                ShipPlacement placement = new ShipPlacement(_currentShip.Name, e.Row, e.Column, _currentOrientation);
                placementsController.add(new PlaceCommand(placemenetsReceiver, placement));
                break;
            case "Move":
                if (tempPl == null)
                {
                    foreach (var p in _placements) if (p.Column == e.Column && p.Row == e.Row) tempPl = p;
                    if (tempPl == null) break;
                    AddLog($"Selected place {tempPl.Name} at {(char)('A' + e.Column)}{e.Row + 1}.");
                }
                else
                {
                    var results1 = _fleet.First(a => a.Name == tempPl.Name);
                    if (!CanPlaceShip(e.Row, e.Column, results1.Length, _currentOrientation))
                    {
                        SystemSounds.Beep.Play();
                        AddLog($"Cannot move {_currentShip.Name} at {(char)('A' + e.Column)}{e.Row + 1}.");
                        return;
                    }
                    AddLog($"New place {tempPl.Name} at {(char)('A' + e.Column)}{e.Row + 1}.");
                    ShipPlacement pp = new ShipPlacement(tempPl.Name, e.Row, e.Column, tempPl.Orientation);
                    placementsController.add(new MoveCommand(placemenetsReceiver, tempPl, pp));
                    tempPl = null;
                }
                break;
            case "Rotate":
                ShipPlacement? pl = null;
                int ind = -1;
                for (int i = 0; i < _placements.Count; i++)
                {
                    var p = _placements[i];
                    if (p.Column == e.Column && p.Row == e.Row) ind = i;
                }
                if (ind == -1) break;
                _playerBoardState[e.Row, e.Column] = CellState.Unknown;
                var results2 = _fleet.First(a => a.Name == _placements[ind].Name);

                if (!CanPlaceShip(e.Row, e.Column, results2.Length, (_placements[ind].Orientation == ShipOrientation.Horizontal ? ShipOrientation.Vertical : ShipOrientation.Horizontal)))
                {
                    SystemSounds.Beep.Play();
                    AddLog($"Cannot rotate {_placements[ind].Name} at {(char)('A' + e.Column)}{e.Row + 1}.");
                    _playerBoardState[e.Row, e.Column] = CellState.Ship;
                    return;
                }
                placementsController.add(new RotateCommand(placemenetsReceiver, _placements[ind]));
                break;


        }
        //_shipQueueControl.MarkPlaced(_currentShip.Name);

        UpdateShipPlacements();
        AdvancePlacementQueue();

        //if (_currentShip is null)
        //{
        //    _ = SubmitPlacementsAsync();
        //}
    }
    private void SubmitPlacements()
    {
        if (_currentShip is null)
        {
            _ = SubmitPlacementsAsync();
        }
        else
        {
            AddLog($"Please place all the ships.");
        }
    }

    private void OpponentBoardOnCellClicked(object? sender, BoardCellEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        if (_stage != GameStage.InProgress || !_isMyTurn || string.IsNullOrEmpty(_client.GameId))
        {
            return;
        }

        var currentState = _opponentBoardState[e.Row, e.Column];
        if (currentState is CellState.Hit or CellState.Miss or CellState.Sunk)
        {
            return;
        }

        _ = FireShotAsync(e.Row, e.Column);
    }

    private async Task SubmitPlacementsAsync()
    {
        try
        {
            var payload = _placements.Select(p => new ShipPlacementDto
            {
                ShipName = p.Name,
                Row = p.Row,
                Column = p.Column,
                Orientation = p.Orientation.ToString()
            }).ToList();

            AddLog("Sending placements to server...");
            await _client.SubmitPlacementsAsync(payload);
            AddLog("Fleet submitted.");
        }
        catch (Exception ex)
        {
            AddLog($"Failed to submit placements: {ex.Message}");
        }
    }

    private async Task FireShotAsync(int row, int column)
    {
        if (_client.GameId is null)
        {
            return;
        }

        try
        {
            AddLog($"Firing at {(char)('A' + column)}{row + 1}...");
            await _client.FireAsync(_client.GameId, row, column,WeaponType);
        }
        catch (Exception ex)
        {
            AddLog($"Shot failed: {ex.Message}");
        }
    }

    private static GameStage MapPhaseToStage(string phase)
    {
        return phase switch
        {
            "WaitingForPlayers" => GameStage.WaitingForOpponent,
            "Placement" => GameStage.Placement,
            "InProgress" => GameStage.InProgress,
            "Completed" => GameStage.Completed,
            _ => GameStage.Connecting
        };
    }

    private bool CanPlaceShip(int row, int column, int length, ShipOrientation orientation)
    {
        if (orientation == ShipOrientation.Horizontal)
        {
            if (column + length > _playerBoardState.Size)
            {
                return false;
            }
        }
        else
        {
            if (row + length > _playerBoardState.Size)
            {
                return false;
            }
        }

        for (var i = 0; i < length; i++)
        {
            var r = orientation == ShipOrientation.Horizontal ? row : row + i;
            var c = orientation == ShipOrientation.Horizontal ? column + i : column;
            if (_playerBoardState[r, c] == CellState.Ship)
            {
                return false;
            }
        }

        return true;
    }

    private void PlaceShip(int row, int column, ShipDefinition ship)
    {
        for (var i = 0; i < ship.Length; i++)
        {
            var r = _currentOrientation == ShipOrientation.Horizontal ? row : row + i;
            var c = _currentOrientation == ShipOrientation.Horizontal ? column + i : column;
            _playerBoardState[r, c] = CellState.Ship;
        }

        //_placements.Add(new ShipPlacement(ship.Name, row, column, _currentOrientation));
        _playerBoardControl.Invalidate();
    }

    private void ApplyShot(ShotResolutionMessage message)
    {
        var isPlayerShooter = string.Equals(message.ShooterId, _client.PlayerId, StringComparison.Ordinal);
        var targetBoard = string.Equals(message.TargetPlayerId, _client.PlayerId, StringComparison.Ordinal)
            ? _playerBoardState
            : _opponentBoardState;

        var row = message.Target.Row;
        var column = message.Target.Column;
        
        var outcome = message.Outcome;
        var cellState = outcome switch
        {
            nameof(Models.ShotOutcome.Hit) or "Hit" => CellState.Hit,
            nameof(Models.ShotOutcome.Sunk) or "Sunk" => CellState.Sunk,
            _ => CellState.Miss
        };
        if (isPlayerShooter && (cellState == CellState.Hit || cellState == CellState.Sunk)) playerPoints++;
        else if ((cellState == CellState.Hit || cellState == CellState.Sunk)) opponentPoints++;
        if (opponentPoints > playerPoints) SetGameState(new LoosingState());
        else SetGameState(new WinningState());
        Coordinate c = new Coordinate(message.Target.Row, message.Target.Column);
        List<Coordinate> coordinates = new List<Coordinate>();
        targetBoard[c.Row, c.Column] = cellState;
        if (message.WeaponType == 1)
        {
            coordinates.Add(new Coordinate(c.Row + 1, c.Column));
            coordinates.Add(new Coordinate(c.Row - 1, c.Column));
            coordinates.Add(new Coordinate(c.Row, c.Column + 1));
            coordinates.Add(new Coordinate(c.Row, c.Column - 1));
        }
        if (message.WeaponType == 2)
        {
            coordinates.Add(new Coordinate(c.Row + 1, c.Column));
            coordinates.Add(new Coordinate(c.Row - 1, c.Column));
            coordinates.Add(new Coordinate(c.Row, c.Column + 1));
            coordinates.Add(new Coordinate(c.Row, c.Column - 1));
            coordinates.Add(new Coordinate(c.Row + 1, c.Column+1));
            coordinates.Add(new Coordinate(c.Row - 1, c.Column-1));
            coordinates.Add(new Coordinate(c.Row - 1, c.Column + 1));
            coordinates.Add(new Coordinate(c.Row + 1, c.Column - 1));
        }
        foreach( var co in coordinates){
            //targetBoard[co.Row, co.Column] = CellState.NotUnknown;
            if (co.Row >= 0 && co.Column >= 0 && co.Row < _playerBoardState.Size &&  co.Column < _playerBoardState.Size)
            {
                if(CellState.Unknown == targetBoard[co.Row, co.Column])
                    targetBoard[co.Row, co.Column] = CellState.NotUnknown;
            }
                
        }

        if (cellState == CellState.Sunk && !string.IsNullOrEmpty(message.ShipName))
        {
            if (string.Equals(message.TargetPlayerId, _client.PlayerId, StringComparison.Ordinal))
            {
                MarkShipAsSunkOnPlayerBoard(message.ShipName);
            }
        }

        _playerBoardControl.Invalidate();
        _opponentBoardControl.Invalidate();

        // Scoreboard will be updated via SignalR observer pattern

        if (isPlayerShooter)
        {
            AddLog($"Shot result: {message.Outcome}{(string.IsNullOrEmpty(message.ShipName) ? string.Empty : $" ({message.ShipName})")}");
        }
        else if (string.Equals(message.TargetPlayerId, _client.PlayerId, StringComparison.Ordinal))
        {
            AddLog($"Your opponent fired at {(char)('A' + column)}{row + 1}: {message.Outcome}");
        }

        if (message.Victory)
        {
            _stage = GameStage.Completed;
            var victory = string.Equals(message.ShooterId, _client.PlayerId, StringComparison.Ordinal);
            AddLog(victory ? "You won the match!" : "You lost the match.");
        }

        if (!string.IsNullOrEmpty(message.NextPlayerId))
        {
            _isMyTurn = string.Equals(message.NextPlayerId, _client.PlayerId, StringComparison.Ordinal);
        }

        Invalidate();
    }

    /* No longer needed - scoreboard updates via Observer pattern through SignalR
    private void UpdateScoreboardFromShot(ShotResolutionMessage message)
    {
        // This method is kept for reference but is no longer used
        // The server now sends ScoreboardDto via observer pattern
    }

    private double CalculateAccuracy(BoardState board)
    {
        // This method is kept for reference but is no longer used
        return 0.0;
    }
    */

    private void MarkShipAsSunkOnPlayerBoard(string shipName)
    {
        var placement = _placements.FirstOrDefault(p => string.Equals(p.Name, shipName, StringComparison.OrdinalIgnoreCase));
        if (placement is null)
        {
            return;
        }

        foreach (var (row, column) in GetShipCoordinates(placement))
        {
            _playerBoardState[row, column] = CellState.Sunk;
        }
    }

    private IEnumerable<(int Row, int Column)> GetShipCoordinates(ShipPlacement placement)
    {
        var definition = _fleet.FirstOrDefault(ship => ship.Name == placement.Name);
        if (definition is null)
        {
            yield break;
        }

        for (var i = 0; i < definition.Length; i++)
        {
            var row = placement.Orientation == ShipOrientation.Horizontal ? placement.Row : placement.Row + i;
            var column = placement.Orientation == ShipOrientation.Horizontal ? placement.Column + i : placement.Column;
            yield return (row, column);
        }
    }

    private void ResetPlacements()
    {
        if (_stage != GameStage.Placement)
        {
            return;
        }
        while (!placementsController.IsEmpty())
        {
            placementsController.undo();

        }
        _placements.Clear();
        _playerBoardControl.Invalidate();
        _playerBoardState.Clear(CellState.Unknown);
        _shipQueueControl.SetFleet(_fleet);
        AdvancePlacementQueue();
        AddLog("Placement reset.");
    }

    private void ToggleOrientation()
    {
        _currentOrientation = _currentOrientation == ShipOrientation.Horizontal
            ? ShipOrientation.Vertical
            : ShipOrientation.Horizontal;
        AddLog($"Orientation set to {_currentOrientation}.");
    }
    //Command
    private void UpdateShipPlacements()
    {
        _playerBoardState.Clear(CellState.Unknown);
        _shipQueueControl.Clear();
        foreach (var p in _placements)
        {
            ShipDefinition sDef = null;
            foreach (var f in _fleet)
            {
                if (f.Name == p.Name) sDef = f;
            }
            if (sDef == null) continue;
            for (var i = 0; i < sDef.Length; i++)
            {
                var r = p.Orientation == ShipOrientation.Horizontal ? p.Row : p.Row + i;
                var c = p.Orientation == ShipOrientation.Horizontal ? p.Column + i : p.Column;
                _playerBoardState[r, c] = CellState.Ship;
            }
            _shipQueueControl.MarkPlaced(p.Name);
        }
        _playerBoardControl.Invalidate();
    }

    private void Place()
    {
        commandType = "Place";
        AddLog($"Command: Place placement.");
    }
    private void Rotate()
    {
        commandType = "Rotate";
        AddLog($"Command: Rotate.");
    }
    private void Move()
    {
        commandType = "Move";
        tempPl = null;
        AddLog($"Command: Move.");
    }
    private void UndoCommand()
    {
        placementsController.undo();

        AddLog($"Undo command.");
        UpdateShipPlacements();
        AdvancePlacementQueue();

    }
    //Template
    private void SetWeapon(int WeaponType)
    {
        this.WeaponType = WeaponType;
    }
    private void ConfigureBoardSizes(int size)
    {
        _playerBoardState = new BoardState(size);
        _opponentBoardState = new BoardState(size);
        _playerBoardState.Clear(CellState.Unknown);
        _opponentBoardState.Clear(CellState.Unknown);
        _playerBoardControl.Board = _playerBoardState;
        _opponentBoardControl.Board = _opponentBoardState;
        
        AdjustWindowSizeForBoard(size);
        
        _playerBoardControl.Invalidate();
        _opponentBoardControl.Invalidate();
    }

    private void AdjustWindowSizeForBoard(int boardSize)
    {
        const int marginSize = 40;
        const int cellSize = 32;
        int boardControlSize = marginSize * 2 + cellSize * boardSize;
        
        _playerBoardControl.Size = new Size(boardControlSize, boardControlSize);
        _opponentBoardControl.Size = new Size(boardControlSize, boardControlSize);
        
        const int leftMargin = 40;
        const int topMargin = 140;
        const int boardSpacing = 20;
        const int rightPanelWidth = 250;
        const int rightPanelMargin = 20;
        const int logHeight = 210;
        const int bottomMargin = 40;
        
        _opponentBoardControl.Location = new Point(
            leftMargin + boardControlSize + boardSpacing,
            topMargin
        );
        
        int rightPanelX = leftMargin + (boardControlSize * 2) + boardSpacing + rightPanelMargin;
        _shipQueueControl.Location = new Point(rightPanelX, topMargin);
        _rotateButton.Location = new Point(rightPanelX, topMargin + 260);
        _resetButton.Location = new Point(rightPanelX, topMargin + 310);
        
        int logY = topMargin + boardControlSize + 20;
        int logWidth = leftMargin + (boardControlSize * 2) + boardSpacing - leftMargin;
        _logControl.Location = new Point(leftMargin, logY);
        _logControl.Size = new Size(logWidth, logHeight);
        
        int scoreboardY = logY + logHeight + 20;
        const int scoreboardHeight = 320;
        _scoreboardControl.Location = new Point(leftMargin, scoreboardY);
        _scoreboardControl.Size = new Size(logWidth, scoreboardHeight);
        
        int windowWidth = rightPanelX + rightPanelWidth + rightPanelMargin;
        int windowHeight = scoreboardY + scoreboardHeight + bottomMargin;
        
        ClientSize = new Size(windowWidth, windowHeight);
        
    }

    private void AddLog(string message)
    {
        _logControl.AddEntry(message);
    }

    private void SafeInvoke(Action action)
    {
        if (InvokeRequired)
        {
            BeginInvoke(action);
        }
        else
        {
            action();
        }
    }

    private void GameForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.R)
        {
            ToggleOrientation();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape && _stage == GameStage.Placement)
        {
            ResetPlacements();
            e.Handled = true;
        }
    }


    public void RenderBoard(BoardState playerBoard, BoardState opponentBoard)
    {
        SafeInvoke(() =>
        {
            _playerBoardState = playerBoard;
            _opponentBoardState = opponentBoard;
            _playerBoardControl.Board = _playerBoardState;
            _opponentBoardControl.Board = _opponentBoardState;
            _playerBoardControl.Invalidate();
            _opponentBoardControl.Invalidate();
            Invalidate();
        });
    }

    public void DisplayMessage(string message)
    {
        SafeInvoke(() => AddLog(message));
    }

    public Task HandlePlayerActionAsync()
    {
        return Task.CompletedTask;
    }

    public void SetGameState(GameState gameState)
    {
        if (this.gameState.GetType() == gameState.GetType()) return;
        //this.gameState.StopMusic();
        this.gameState = gameState;
        this.gameState.PlayMusic();

        //determine
    }

}
