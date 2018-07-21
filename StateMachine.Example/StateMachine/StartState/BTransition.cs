using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class BTransition : Transition<ExampleStateMachine, ExampleStates, StartState, string, StartState>
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