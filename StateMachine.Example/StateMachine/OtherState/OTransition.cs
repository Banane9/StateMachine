using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.OtherState
{
    public class OTransition : ExampleTransition<OtherState, StartState.StartState>
    {
        public override bool CanTransition(ExampleTransitionAttempt<OtherState> attempt)
        {
            return true;
        }

        public override StartState.StartState DoTransition(ExampleTransitionAttempt<OtherState> attempt)
        {
            attempt.Machine.Print("You wrote literally anything! Going back to the start state.");

            return new StartState.StartState();
        }
    }
}