using Battleship.Client.Models;
using Battleship.Client.Models.Command;
using Battleship.Client.Services;
using Battleship.Client.UI;
using System.Media;

namespace Battleship.Client.Patterns.Mediator;

public class PlacementMediator : IPlacementMediator
{
    private readonly BoardControl _boardControl;
    private readonly ShipQueueControl _shipQueueControl;
    private readonly LogControl _logControl;
    private readonly ActionButton _revertButton;
    private readonly ActionButton _resetButton;
    
    private readonly PlacementController _placementsController;
    private readonly ShipPlacements _placementsReceiver;
    private readonly List<ShipPlacement> _placements;
    private readonly List<PositionMemento> _positionMementos;
    private BoardState _boardState;
    
    private readonly List<ShipDefinition> _fleet = new();
    private ShipDefinition? _currentShip;
    private ShipOrientation _currentOrientation = ShipOrientation.Horizontal;
    private string _commandType = "Place";
    private ShipPlacement? _tempPl = null;

    public bool IsPlacementComplete => _currentShip == null && _fleet.Count > 0;

    public PlacementMediator(
        BoardControl boardControl,
        ShipQueueControl shipQueueControl,
        LogControl logControl,
        ActionButton revertButton,
        ActionButton resetButton,
        PlacementController placementsController,
        List<ShipPlacement> placements,
        List<PositionMemento> positionMementos,
        BoardState boardState)
    {
        _boardControl = boardControl;
        _shipQueueControl = shipQueueControl;
        _logControl = logControl;
        _revertButton = revertButton;
        _resetButton = resetButton;
        _placementsController = placementsController;
        _placements = placements;
        _positionMementos = positionMementos;
        _boardState = boardState;
        _placementsReceiver = new ShipPlacements(_placements);
    }

    public void StartPlacementPhase(IEnumerable<ShipDefinition> fleet)
    {
        _fleet.Clear();
        _fleet.AddRange(fleet);
        _shipQueueControl.SetFleet(_fleet);
        
        _placements.Clear();
        _positionMementos.Clear();
        _boardState.Clear(CellState.Unknown);
        _boardControl.Board = _boardState;
        
        AdvancePlacementQueue();
        
        _logControl.AddEntry("Place your ships on the left board. Left click to place, right click to rotate, or press R to rotate.");
        _logControl.AddEntry("Use 'Revert last ship' button or ESC to undo, 'Reset placement' to start over.");
    }

    public void SetCommandType(string type)
    {
        _commandType = type;
        _tempPl = null;
        _logControl.AddEntry($"Command: {type}.");
    }

    public void HandleRotate()
    {
        _currentOrientation = _currentOrientation == ShipOrientation.Horizontal
            ? ShipOrientation.Vertical
            : ShipOrientation.Horizontal;
        _logControl.AddEntry($"Orientation set to {_currentOrientation}.");
    }

    public void HandleReset()
    {
        SaveMemento();
        
        while (!_placementsController.IsEmpty())
        {
            _placementsController.undo();
        }
        
        _placements.Clear();
        _boardState.Clear(CellState.Unknown);
        _boardControl.Board = _boardState;
        _boardControl.Invalidate();
        
        _shipQueueControl.SetFleet(_fleet);
        
        AdvancePlacementQueue();
        UpdatePlacementButtons();
        _logControl.AddEntry("Placement reset. Use 'Revert last action' to undo this reset.");
    }

    public void HandleRevert()
    {
        if (_positionMementos.Count == 0)
        {
            _logControl.AddEntry("No placements to revert.");
            return;
        }

        var previousMemento = _positionMementos[_positionMementos.Count - 1];
        _positionMementos.RemoveAt(_positionMementos.Count - 1);

        _placements.Clear();
        _placements.AddRange(previousMemento.GetSavedPlacements());
        
        // Restore board state
        var savedBoard = previousMemento.GetBoardState();
        for(int r=0; r<savedBoard.Size; r++)
            for(int c=0; c<savedBoard.Size; c++)
                _boardState[r,c] = savedBoard[r,c];

        _placementsController.RestoreCommandStack(previousMemento.GetCommandStack());

        _boardControl.Board = _boardState;
        
        UpdateShipPlacements();
        
        _shipQueueControl.SetFleet(_fleet);
        foreach (var placement in _placements)
        {
            _shipQueueControl.MarkPlaced(placement.Name);
        }

        AdvancePlacementQueue();
        UpdatePlacementButtons();
        _logControl.AddEntry("Reverted to previous state.");
    }

    public void HandleCellClick(int row, int column, MouseButtons button)
    {
        if (button == MouseButtons.Right)
        {
            HandleRotate();
            return;
        }

        switch (_commandType)
        {
            case "Place":
                if (_currentShip is null)
                {
                    _logControl.AddEntry($"All fleet is in place.");
                    return;
                }
                if (!CanPlaceShip(row, column, _currentShip.Length, _currentOrientation))
                {
                    SystemSounds.Beep.Play();
                    _logControl.AddEntry($"Cannot place {_currentShip.Name} at {(char)('A' + column)}{row + 1}.");
                    return;
                }
                SaveMemento();
                ShipPlacement placement = new ShipPlacement(_currentShip.Name, row, column, _currentOrientation);
                _placementsController.add(new PlaceCommand(_placementsReceiver, placement));
                break;
            case "Move":
                if (_tempPl == null)
                {
                    foreach (var p in _placements) if (p.Column == column && p.Row == row) _tempPl = p;
                    if (_tempPl == null) break;
                    _logControl.AddEntry($"Selected place {_tempPl.Name} at {(char)('A' + column)}{row + 1}.");
                }
                else
                {
                    var results1 = _fleet.First(a => a.Name == _tempPl.Name);
                    if (!CanPlaceShip(row, column, results1.Length, _currentOrientation))
                    {
                        SystemSounds.Beep.Play();
                        _logControl.AddEntry($"Cannot move {_currentShip?.Name ?? "ship"} at {(char)('A' + column)}{row + 1}.");
                        return;
                    }
                    _logControl.AddEntry($"New place {_tempPl.Name} at {(char)('A' + column)}{row + 1}.");
                    SaveMemento();
                    ShipPlacement pp = new ShipPlacement(_tempPl.Name, row, column, _tempPl.Orientation);
                    _placementsController.add(new MoveCommand(_placementsReceiver, _tempPl, pp));
                    _tempPl = null;
                }
                break;
            case "Rotate":
                int ind = -1;
                for (int i = 0; i < _placements.Count; i++)
                {
                    var p = _placements[i];
                    if (p.Column == column && p.Row == row) 
                    {
                        ind = i;
                        break;
                    }
                }
                if (ind == -1) 
                {
                    _logControl.AddEntry($"No ship found at {(char)('A' + column)}{row + 1}.");
                    break;
                }
                
                var shipToRotate = _placements[ind];
                var results2 = _fleet.First(a => a.Name == shipToRotate.Name);
                var newOrientation = shipToRotate.Orientation == ShipOrientation.Horizontal 
                    ? ShipOrientation.Vertical 
                    : ShipOrientation.Horizontal;

                // Temporarily clear the ship's current position for validation
                for (var i = 0; i < results2.Length; i++)
                {
                    var r = shipToRotate.Orientation == ShipOrientation.Horizontal ? shipToRotate.Row : shipToRotate.Row + i;
                    var c = shipToRotate.Orientation == ShipOrientation.Horizontal ? shipToRotate.Column + i : shipToRotate.Column;
                    _boardState[r, c] = CellState.Unknown;
                }

                if (!CanPlaceShip(shipToRotate.Row, shipToRotate.Column, results2.Length, newOrientation))
                {
                    SystemSounds.Beep.Play();
                    _logControl.AddEntry($"Cannot rotate {shipToRotate.Name} at {(char)('A' + column)}{row + 1}.");
                    // Restore the ship's original position
                    for (var i = 0; i < results2.Length; i++)
                    {
                        var r = shipToRotate.Orientation == ShipOrientation.Horizontal ? shipToRotate.Row : shipToRotate.Row + i;
                        var c = shipToRotate.Orientation == ShipOrientation.Horizontal ? shipToRotate.Column + i : shipToRotate.Column;
                        _boardState[r, c] = CellState.Ship;
                    }
                    return;
                }
                
                SaveMemento();
                _placementsController.add(new RotateCommand(_placementsReceiver, shipToRotate));
                _logControl.AddEntry($"Rotated {shipToRotate.Name} to {newOrientation}.");
                break;
        }

        UpdateShipPlacements();
        AdvancePlacementQueue();
    }

    private void AdvancePlacementQueue()
    {
        _currentShip = _fleet.FirstOrDefault(ship => _placements.All(p => !string.Equals(p.Name, ship.Name, StringComparison.OrdinalIgnoreCase)));
        _shipQueueControl.SetCurrentShip(_currentShip?.Name);
        UpdatePlacementButtons();
    }

    private void UpdatePlacementButtons()
    {
        _revertButton.Enabled = _positionMementos.Count > 0;
        _resetButton.Enabled = _placements.Count > 0;
    }

    private void UpdateShipPlacements()
    {
        _boardState.Clear(CellState.Unknown);
        _shipQueueControl.Clear();
        foreach (var p in _placements)
        {
            ShipDefinition? sDef = null;
            foreach (var f in _fleet)
            {
                if (f.Name == p.Name) sDef = f;
            }
            if (sDef == null) continue;
            for (var i = 0; i < sDef.Length; i++)
            {
                var r = p.Orientation == ShipOrientation.Horizontal ? p.Row : p.Row + i;
                var c = p.Orientation == ShipOrientation.Horizontal ? p.Column + i : p.Column;
                _boardState[r, c] = CellState.Ship;
            }
            _shipQueueControl.MarkPlaced(p.Name);
        }
        _boardControl.Invalidate();
    }

    private bool CanPlaceShip(int row, int column, int length, ShipOrientation orientation)
    {
        if (orientation == ShipOrientation.Horizontal)
        {
            if (column + length > _boardState.Size)
            {
                return false;
            }
        }
        else
        {
            if (row + length > _boardState.Size)
            {
                return false;
            }
        }

        for (var i = 0; i < length; i++)
        {
            var r = orientation == ShipOrientation.Horizontal ? row : row + i;
            var c = orientation == ShipOrientation.Horizontal ? column + i : column;
            if (_boardState[r, c] == CellState.Ship)
            {
                return false;
            }
        }

        return true;
    }

    private void SaveMemento()
    {
        var memento = new PositionMemento(_placements, _boardState, _placementsController.GetCommandStack());
        _positionMementos.Add(memento);
    }
}
