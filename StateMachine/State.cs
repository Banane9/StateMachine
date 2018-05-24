using System;
using System.Collections.Generic;

namespace StateMachine
{
    public sealed class State<TState, TStateMachine> where TStateMachine : StateMachine<TState, >
    {
    }
}