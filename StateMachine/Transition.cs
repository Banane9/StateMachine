using System;
using System.Collections.Generic;

namespace StateMachine
{
    public abstract class Transition<TMachine, TStates, TStateIn, TWith, TStateOut>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
        where TStateOut : TStates
    {
        public abstract bool CanTransition(TransitionAttempt<TMachine, TStateIn, TWith> attempt);

        public abstract TStateOut DoTransition(TransitionAttempt<TMachine, TStateIn, TWith> attempt);
    }
}