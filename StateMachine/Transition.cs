using System;
using System.Collections.Generic;

namespace StateMachine
{
    /// <summary>
    /// Abstract base class for all transitions. For simplicity,
    /// it's advisable to create another abstract class that pre-fills everything but TStateIn and TStateOut.
    /// </summary>
    /// <typeparam name="TMachine">The type of the <see cref="StateMachine{TStates, TWith}"/> that's used with
    /// the <see cref="TransitionAttempt{TMachine, TStates, TStateIn, TWith}"/>.</typeparam>
    /// <typeparam name="TStates">The base type of the states used by the machine.</typeparam>
    /// <typeparam name="TStateIn">The base type of the state that the machine must be in to use this transition.</typeparam>
    /// <typeparam name="TWith">The base type of the inputs of the machine.</typeparam>
    /// <typeparam name="TStateOut">The base type of the state that this transition leads to.</typeparam>
    /// <typeparam name="TTransitionAttempt">The base type of the
    /// <see cref="TransitionAttempt{TMachine, TStates, TStateIn, TWith}"/> that contains all the details.</typeparam>
    public abstract class Transition<TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt>
        where TMachine : StateMachine<TStates, TWith>
        where TStates : MachineState
        where TStateIn : TStates
        where TStateOut : TStates
        where TTransitionAttempt : TransitionAttempt<TMachine, TStates, TStateIn, TWith>, new()
    {
        /// <summary>
        /// Determines whether this transition can be taken with the current state and input.
        /// </summary>
        /// <param name="attempt"><see cref="TransitionAttempt{TMachine, TStates, TStateIn, TWith}"/> containing the details of the attempt.</param>
        /// <returns>Whether this transition can be taken with the current state and input.</returns>
        public abstract bool CanTransition(TTransitionAttempt attempt);

        /// <summary>
        /// Executes this transition with the current state and input.
        /// </summary>
        /// <param name="attempt"><see cref="TransitionAttempt{TMachine, TStates, TStateIn, TWith}"/> containing the details of the attempt.</param>
        /// <returns>The new state of the machine.</returns>
        public abstract TStateOut DoTransition(TTransitionAttempt attempt);
    }
}