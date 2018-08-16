using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine.StartState
{
    public class ATransition : ExampleTransition<StartState, OtherState.OtherState>
    {
        public override bool CanTransition(ExampleStateMachine machine, StartState state, string with)
        {
            return with.Length > 5;
        }

        public override OtherState.OtherState DoTransition(ExampleStateMachine machine, StartState state, string with)
        {
            machine.Print("You wrote something with more than 5 characters! Now going into the other state.");

            return new OtherState.OtherState();
        }
    }
}