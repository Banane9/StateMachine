using System;
using System.Collections.Generic;
using StateMachine.Example.StateMachine;
using StateMachine.Example.StateMachine.StartState;

namespace StateMachine.Example
{
    public sealed class ExampleStateMachine : StateMachine<ExampleStates, string>
    {
        public ExampleStateMachine()
            : base(new StartState())
        { }

        public void Print(string s) => Console.WriteLine(s);
    }
}