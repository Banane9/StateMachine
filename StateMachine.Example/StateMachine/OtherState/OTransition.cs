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
            return new StartState.StartState();
        }
    }
}