using System;

namespace Battleship.Server.Interpreter
{
    public class StopExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            Console.WriteLine("Stopping server...");
            context.Lifetime.StopApplication();
        }
    }
}
