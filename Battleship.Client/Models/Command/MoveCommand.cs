using Battleship.Client.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    internal class MoveCommand : ICommand
    {
        private IReceiver receiver;

        private ShipPlacement oldPlacement;
        private ShipPlacement newPlacement;
        public MoveCommand(IReceiver receiver, ShipPlacement oldPlacement, ShipPlacement newPlacement)
        {
            this.receiver = receiver;
            this.oldPlacement = oldPlacement;
            this.newPlacement = newPlacement;
        }

        public override void Execute()
        {
            receiver.Move(oldPlacement, newPlacement);
            
        }

        public override void Undo()
        {
            receiver.Move(newPlacement,oldPlacement);
        }
    }
}
