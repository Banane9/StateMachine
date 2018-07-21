namespace StateMachine
{
    public interface IStateMachine<in TStates, TWith>
        where TStates : MachineState
    {
    }
}