using System;
using System.Collections.Generic;
using StateMachine.Example.StateMachine;

namespace StateMachine.Example
{
    public sealed class ExampleStateMachine : StateMachine<ExampleStates, string>
    {
        public void Print(string s) => Console.WriteLine(s);
    }
}