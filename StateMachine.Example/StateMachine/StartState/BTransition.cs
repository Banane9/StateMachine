using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class BTransition : ExampleTransition<StartState, StartState>
    {
        public override bool CanTransition(ExampleStateMachine machine, StartState state, string with)
        {
            return with.Length <= 5;
        }

        public override StartState DoTransition(ExampleStateMachine machine, StartState state, string with)
        {
            machine.Print("You wrote something with no more than 5 characters! Staying in the start state.");

            return state;
        }
    }
}