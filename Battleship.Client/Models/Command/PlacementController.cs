using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.Models.Command
{
    public  class PlacementController
    {
        private readonly Stack<ICommand> commands;
        public PlacementController() {
            commands = new Stack<ICommand>();
        }
        public bool IsEmpty() { 
            return commands.Count == 0;
        }
        public void run()
        {
            
        }
        public void add(ICommand command)
        {
            commands.Push(command);
            command.Execute();
        }
        public void undo() {
            if (commands.Count > 0)
            {
                ICommand command = commands.Pop();
                command.Undo();
            }
        }

        public Stack<ICommand> GetCommandStack()
        {
            return new Stack<ICommand>(commands.Reverse());
        }

        public void RestoreCommandStack(Stack<ICommand> commandStack)
        {
            commands.Clear();
            foreach (var command in commandStack.Reverse())
            {
                commands.Push(command);
            }
        }
    }
}
