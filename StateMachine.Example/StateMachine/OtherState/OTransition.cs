using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.OtherState
{
    public class OTransition : ExampleTransition<OtherState, StartState.StartState>
    {
        public override bool CanTransition(ExampleStateMachine machine, OtherState state, string with)
        {
            return true;
        }

        public override StartState.StartState DoTransition(ExampleStateMachine machine, OtherState state, string with)
        {
            machine.Print("You wrote literally anything! Going back to the start state.");

            return new StartState.StartState();
        }
    }
}