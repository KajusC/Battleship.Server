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
            int index = -1;
            
            for (int i = 0; i < _placements.Count; i++)
            {
                if (_placements[i] == shipPlacement)
                {
                    index = i;
                    break;
                }
            }
            
            if (index == -1) return;
            
            // Create a new ShipPlacement with rotated orientation instead of modifying the existing one
            var oldPlacement = _placements[index];
            var newOrientation = oldPlacement.Orientation == ShipOrientation.Horizontal
                    ? ShipOrientation.Vertical
                    : ShipOrientation.Horizontal;
            var newPlacement = new ShipPlacement(oldPlacement.Name, oldPlacement.Row, oldPlacement.Column, newOrientation);
            _placements[index] = newPlacement;
        }
    }
}
