using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Client.DesignPatterns
{
        // The Command interface declares a method for executing a command.
        public interface ICommand
        {
        //SelectShip
        //SelectWeapon
        //SelectTarget
        //UsePowerUp
        //EndTurn
            void Execute();
        }

        // Some commands can implement simple operations on their own.
        class CommandShoot : ICommand
        {
            private int x,y;
            private object weapon;
            public CommandShoot(int _x, int _y, object weapon)
        {
            x = _x;
            y = _y;
            this.weapon = weapon;
        }

            public void Execute()
            {
                //send to server x, y, weapon damage and size
                Console.WriteLine($"CommandShoot: send to server (x:{x}, y:{y}, w_damage: ,w_size: )");
            }
        }
        
        // However, some commands can delegate more complex operations to other
        // objects, called "receivers."
        class ComplexCommand : ICommand
        {
            private Receiver _receiver;

            // Context data, required for launching the receiver's methods.
            private string _a;

            private string _b;

            // Complex commands can accept one or several receiver objects along
            // with any context data via the constructor.
            public ComplexCommand(Receiver receiver, string a, string b)
            {
                _receiver = receiver;
                _a = a;
                _b = b;
            }

            // Commands can delegate to any methods of a receiver.
            public void Execute()
            {
                Console.WriteLine("ComplexCommand: Complex stuff should be done by a receiver object.");
                _receiver.DoSomething(_a);
                _receiver.DoSomethingElse(_b);
            }
        }

        // The Receiver classes contain some important business logic. They know how
        // to perform all kinds of operations, associated with carrying out a
        // request. In fact, any class may serve as a Receiver.
        class Receiver
        {
            public void DoSomething(string a)
            {
                Console.WriteLine($"Receiver: Working on ({a}.)");
            }

            public void DoSomethingElse(string b)
            {
                Console.WriteLine($"Receiver: Also working on ({b}.)");
            }
        }

        // The Invoker is associated with one or several commands. It sends a
        // request to the command.
        class Invoker
        {
            private ICommand _command;
            // Initialize commands.
            public void SetCommand(ICommand command)
            {
                _command = command;
            }
            public void DoCommand()
            {
                _command.Execute();
            }
        
        }

        class Program3
        {
            static void Main1(string[] args)
            {
                // The client code can parameterize an invoker with any commands.
                Invoker invoker = new Invoker();
                invoker.SetCommand(new CommandShoot(4,5, new object()));
//                Receiver receiver = new Receiver();
//                invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));

                invoker.DoCommand();
            }
        }
}