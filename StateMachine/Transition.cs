using System;
using System.Collections.Generic;

namespace StateMachine
{
    public abstract class Transition<TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
        where TStateOut : TStates
        where TTransitionAttempt : TransitionAttempt<TMachine, TStates, TStateIn, TWith>, new()
    {
        public abstract bool CanTransition(TTransitionAttempt attempt);

        public abstract TStateOut DoTransition(TTransitionAttempt attempt);
    }
}