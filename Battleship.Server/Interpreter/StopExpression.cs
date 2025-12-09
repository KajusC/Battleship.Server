using System;

namespace Battleship.Server.Interpreter
{
    public class StopExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            context.Output = "Stopping server...";
            context.Lifetime.StopApplication();
        }
    }
}
