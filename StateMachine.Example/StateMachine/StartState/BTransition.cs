using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class BTransition : ExampleTransition<StartState, StartState>
    {
        public override bool CanTransition(ExampleTransitionAttempt<StartState> attempt)
        {
            return attempt.With.Length <= 5;
        }

        public override StartState DoTransition(ExampleTransitionAttempt<StartState> attempt)
        {
            return attempt.State;
        }
    }
}