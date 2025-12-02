using Battleship.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{

    
    public class PlaceCommand : ICommand
    {
        private ShipPlacement placement;
        private IReceiver receiver;

        public PlaceCommand(IReceiver receiver, ShipPlacement placement)
        {
            this.receiver = receiver;
            this.placement = placement;
        }


        public override void Execute()
        {
            receiver.Add(placement);
        }



        public override void Undo()
        {
            receiver.Remove(placement);
        }
    }
}
