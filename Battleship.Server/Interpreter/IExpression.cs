using System;

namespace Battleship.Server.Interpreter
{
    public interface IExpression
    {
        void Interpret(InterpreterContext context);
    }
}