using Battleship.Client.Models;
using System.Windows.Forms;

namespace Battleship.Client.Patterns.Mediator;

public interface IPlacementMediator
{
    void HandleCellClick(int row, int column, MouseButtons button);
    void HandleRotate();
    void HandleReset();
    void HandleRevert();
    void SetCommandType(string type);
    void StartPlacementPhase(IEnumerable<ShipDefinition> fleet);
    bool IsPlacementComplete { get; }
}
