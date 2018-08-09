using System;
using System.Collections.Generic;

namespace StateMachine
{
    public class TransitionAttempt<TMachine, TStates, TStateIn, TWith>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
    {
        public TMachine Machine { get; set; }

        public TStateIn State { get; set; }

        public TWith With { get; set; }
    }
}