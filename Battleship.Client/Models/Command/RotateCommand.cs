using Battleship.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    internal class RotateCommand : ICommand
    {
        private ShipPlacement oldPlacement;
        private IReceiver receiver;

        public RotateCommand(IReceiver receiver,ShipPlacement oldPlacement)
        {
            this.receiver = receiver;
            this.oldPlacement = oldPlacement;
        }

        public override void Execute() {
            receiver.Rotate(oldPlacement);
            
        }
        public override void Undo() {
            receiver.Rotate(oldPlacement);
        }
    }
}
