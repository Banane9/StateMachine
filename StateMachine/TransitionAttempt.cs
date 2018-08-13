namespace StateMachine
{
    /// <summary>
    /// Represents a very generic transition attempt.
    /// Fully usable, but can be derived from for brevity's sake.
    /// </summary>
    /// <typeparam name="TMachine">The type of the <see cref="StateMachine{TStates, TWith}"/> that's used with
    /// the <see cref="TransitionAttempt{TMachine, TStates, TStateIn, TWith}"/>.</typeparam>
    /// <typeparam name="TStates">The base type of the states used by the machine.</typeparam>
    /// <typeparam name="TStateIn">The base type of the state that the machine must be in to use this transition.</typeparam>
    /// <typeparam name="TWith">The base type of the inputs of the machine.</typeparam>
    public class TransitionAttempt<TMachine, TStates, TStateIn, TWith>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
    {
        /// <summary>
        /// Gets the machine that the transition is executed by.
        /// </summary>
        public TMachine Machine { get; set; }

        /// <summary>
        /// Gets the current state of the machine that the transition is executed by.
        /// </summary>
        public TStateIn State { get; set; }

        /// <summary>
        /// Gets the input that the transition is executed with.
        /// </summary>
        public TWith With { get; set; }
    }
}