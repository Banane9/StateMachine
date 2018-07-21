using System;

namespace StateMachine
{
    public class StateMachine<TStates, TWith> : IStateMachine<TStates, TWith>
        where TStates : MachineState
    {
    }
}