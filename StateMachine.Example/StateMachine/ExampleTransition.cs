using System;
using System.Collections.Generic;

namespace StateMachine.Example.StateMachine
{
    public abstract class ExampleTransition<TStateIn, TStateOut> : Transition<ExampleStateMachine, ExampleStates, TStateIn, string, TStateOut>
        where TStateIn : ExampleStates
        where TStateOut : ExampleStates
    {
    }
}