using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class ATransition : ExampleTransition<StartState, OtherState.OtherState>
    {
        public override bool CanTransition(ExampleTransitionAttempt<StartState> attempt)
        {
            return attempt.With.Length > 5;
        }

        public override OtherState.OtherState DoTransition(ExampleTransitionAttempt<StartState> attempt)
        {
            return new OtherState.OtherState();
        }
    }
}