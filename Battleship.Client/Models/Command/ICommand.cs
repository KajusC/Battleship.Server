using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    public abstract class ICommand
    {
        private IReceiver receiver;
        public abstract void Execute();
        public abstract void Undo();
    }
}
