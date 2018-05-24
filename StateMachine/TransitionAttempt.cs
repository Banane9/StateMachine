using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachine
{
    public sealed class TransitionAttempt<TState, TWith>
    {
        public TState State { get; }

        public TWith With { get; }

        public TransitionAttempt(TState state, TWith with)
        {
            State = state;
            With = with;
        }
    }
}
