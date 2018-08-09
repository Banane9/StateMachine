using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine
{
    public sealed class ExampleTransitionAttempt<TStateIn> : TransitionAttempt<ExampleStateMachine, ExampleStates, TStateIn, string>
        where TStateIn : ExampleStates
    { }
}