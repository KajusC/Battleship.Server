using Battleship.Client.Models;
using Battleship.Client.Models.Command;

namespace Battleship.Client.Services
{
    public class PositionMemento
    {
        private readonly List<ShipPlacement> _placementList;
        private readonly BoardState _boardState;
        private readonly Stack<ICommand> _commandStack;

        public PositionMemento(List<ShipPlacement> placements, BoardState boardState, Stack<ICommand> commandStack)
        {
            _placementList = placements.Select(p => new ShipPlacement(p.Name, p.Row, p.Column, p.Orientation)).ToList();
            _boardState = boardState.Clone();
            _commandStack = new Stack<ICommand>(commandStack.Reverse());
        }

        public List<ShipPlacement> GetSavedPlacements()
        {
            return _placementList.Select(p => new ShipPlacement(p.Name, p.Row, p.Column, p.Orientation)).ToList();
        }

        public BoardState GetBoardState()
        {
            return _boardState.Clone();
        }

        public Stack<ICommand> GetCommandStack()
        {
            return new Stack<ICommand>(_commandStack.Reverse());
        }
    }
}
