namespace StateMachine
{
    /// <summary>
    /// Abstract base class for all transitions. For simplicity,
    /// it's advisable to create another abstract class that pre-fills everything but TStateIn and TStateOut.
    /// </summary>
    /// <typeparam name="TMachine">The type of the <see cref="StateMachine{TStates, TWith}"/> that executes the transition.</typeparam>
    /// <typeparam name="TStates">The base type of the states used by the machine.</typeparam>
    /// <typeparam name="TStateIn">The base type of the state that the machine must be in to use this transition.</typeparam>
    /// <typeparam name="TWith">The base type of the inputs of the machine.</typeparam>
    /// <typeparam name="TStateOut">The base type of the state that this transition leads to.</typeparam>
    public abstract class Transition<TMachine, TStates, TStateIn, TWith, TStateOut>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
        where TStateOut : TStates
    {
        /// <summary>
        /// Determines whether this transition can be taken with the current state and input.
        /// </summary>
        /// <param name="machine">The machine attempting the transition.</param>
        /// <param name="state">The state that the machine is in.</param>
        /// <param name="with">The input that the transition is being attempted with.</param>
        /// <returns>Whether this transition can be taken with the current state and input.</returns>
        public abstract bool CanTransition(TMachine machine, TStateIn state, TWith with);

        /// <summary>
        /// Executes this transition with the current state and input.
        /// </summary>
        /// <param name="machine">The machine attempting the transition.</param>
        /// <param name="state">The state that the machine is in.</param>
        /// <param name="with">The input that the transition is being attempted with.</param>
        /// <returns>The new state of the machine.</returns>
        public abstract TStateOut DoTransition(TMachine machine, TStateIn state, TWith with);
    }
}