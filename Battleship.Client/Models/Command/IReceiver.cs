using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    public interface IReceiver
    {
        public void Add(ShipPlacement shipPlacement);
        public void Remove(ShipPlacement shipPlacement);
        public void Rotate(ShipPlacement shipPlacement);
        public void Move(ShipPlacement oldPlacement, ShipPlacement newPlacement);

    }
}
