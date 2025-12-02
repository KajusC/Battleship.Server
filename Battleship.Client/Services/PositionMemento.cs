using Battleship.Client.Models;

namespace Battleship.Client.Services
{
    internal class PositionMemento
    {
        private readonly List<ShipPlacement> _placementList;
        private readonly BoardState _boardState;

        public PositionMemento(List<ShipPlacement> placements, BoardState boardState)
        {
            _placementList = [.. placements];
            _boardState = boardState.Clone();
        }

        public List<ShipPlacement> GetSavedPlacements()
        {
            return [.. _placementList];
        }

        public BoardState GetBoardState()
        {
            return _boardState.Clone();
        }
    }
}
