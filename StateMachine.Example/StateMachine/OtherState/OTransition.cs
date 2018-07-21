﻿using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.OtherState
{
    public class OTransition : Transition<ExampleStateMachine, ExampleStates, OtherState, string, StartState.StartState>
    {
        public override bool CanTransition(TransitionAttempt<ExampleStateMachine, OtherState, string> attempt)
        {
            return true;
        }

        public override StartState.StartState DoTransition(TransitionAttempt<ExampleStateMachine, OtherState, string> attempt)
        {
            return new StartState.StartState();
        }
    }
}