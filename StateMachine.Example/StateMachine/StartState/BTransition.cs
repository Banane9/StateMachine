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
            attempt.Machine.Print("You wrote something with no more than 5 characters! Staying in the start state.");

            return attempt.State;
        }
    }
}