using System;
using System.Collections.Generic;

namespace StateMachine
{
    public sealed class TransitionAttempt<TMachine, TState, TWith>
        where TMachine : IStateMachine<TState, TWith>
        where TState : MachineState
    {
        public TMachine Machine { get; }

        public TState State { get; }

        public TWith With { get; }

        public TransitionAttempt(TMachine machine, TState state, TWith with)
        {
            Machine = machine;
            State = state;
            With = with;
        }
    }
}