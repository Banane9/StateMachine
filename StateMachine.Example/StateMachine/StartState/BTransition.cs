using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class BTransition : ExampleTransition<StartState, StartState>
    {
        public override bool CanTransition(TransitionAttempt<ExampleStateMachine, StartState, string> attempt)
        {
            return attempt.With.Length <= 5;
        }

        public override StartState DoTransition(TransitionAttempt<ExampleStateMachine, StartState, string> attempt)
        {
            return attempt.State;
        }
    }
}