using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    internal class ShipPlacements : IReceiver
    {
        private List<ShipPlacement> _placements;
        public ShipPlacements(List<ShipPlacement> _placements) { 
            this._placements = _placements;
        }
        public void Add(ShipPlacement placement)
        {
            _placements.Add(placement);
        }

        public void Move(ShipPlacement oldPlacement, ShipPlacement newPlacement)
        {
            for (int i = 0; i < _placements.Count; i++)
                if (_placements[i] == oldPlacement) _placements[i] = newPlacement;
        }

        public void Remove(ShipPlacement placement)
        {
            _placements.Remove(placement);
        }

        public void Rotate(ShipPlacement shipPlacement)
        {
            ShipPlacement placement = null;
            
            for (int i = 0; i < _placements.Count; i++)
                if (_placements[i] == shipPlacement) placement = _placements[i];
            if (placement == null) return;
            placement.Orientation = placement.Orientation == ShipOrientation.Horizontal
                    ? ShipOrientation.Vertical
                    : ShipOrientation.Horizontal;
        }
    }
}
