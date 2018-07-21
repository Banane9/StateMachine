using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class ATransition : Transition<ExampleStateMachine, ExampleStates, StartState, string, OtherState.OtherState>
    {
        public override bool CanTransition(TransitionAttempt<ExampleStateMachine, StartState, string> attempt)
        {
            return attempt.With.Length > 5;
        }

        public override OtherState.OtherState DoTransition(TransitionAttempt<ExampleStateMachine, StartState, string> attempt)
        {
            return new OtherState.OtherState();
        }
    }
}