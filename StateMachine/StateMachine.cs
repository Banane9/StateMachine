using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StateMachine
{
    /// <summary>
    /// Represents the abstract base for all state machines. Contains the logic that looks for transitions and handles them.
    /// </summary>
    /// <typeparam name="TStates">The base type of the states used by this machine.</typeparam>
    /// <typeparam name="TWith">The base type of the transition inputs used by this machine.</typeparam>
    public abstract class StateMachine<TStates, TWith>
        where TStates : MachineState
    {
        private static readonly Dictionary<HashSet<Assembly>, Dictionary<Type, TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>[]>> knownAssemblies =
            new Dictionary<HashSet<Assembly>, Dictionary<Type, TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>[]>>(new HashSetEqualityComparer<Assembly>());

        private static readonly Type withType = typeof(TWith);
        private readonly TStates startState;
        private readonly Type thisType;

        private readonly Dictionary<Type, TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>[]> transitions;
        private TStates currentState;

        /// <summary>
        /// Gets the current state of the <see cref="StateMachine{TStates, TWith}"/>.
        /// </summary>
        public TStates CurrentState
        {
            get => currentState;
            private set => currentState = value ?? throw new ArgumentNullException(nameof(value), "Current State can't be null!");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class,
        /// looking for matching <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the
        /// <see cref="Assembly"/> where the deriving class is defined.
        /// </summary>
        /// <param name="startState">The state that the state machine starts in.</param>
        protected StateMachine(TStates startState)
        {
            thisType = GetType();

            this.startState = startState;
            CurrentState = startState;

            transitions = buildStateMachine(thisType.Assembly);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class, looking for matching
        /// <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the given <see cref="Assembly"/>s.
        /// </summary>
        /// <param name="startState">The state that the state machine starts in.</param>
        /// <param name="assemblies">The <see cref="Assembly"/>s to look for matching Transitions in.</param>
        protected StateMachine(TStates startState, params Assembly[] assemblies)
        {
            thisType = GetType();

            this.startState = startState;
            CurrentState = startState;

            transitions = buildStateMachine(assemblies);
        }

        /// <summary>
        /// Sets the current state of the machine to the given one.
        /// </summary>
        /// <param name="state">The new state to use as the current state.</param>
        public void ForceState(TStates state) => CurrentState = state;

        /// <summary>
        /// Tries transitioning from the current state with the given input.
        /// Tries all transitions for the current type of the state, up to the base state type of the machine.
        /// </summary>
        /// <param name="with">The input to try and transition with.</param>
        /// <returns>Whether a transition was successfully executed or not.</returns>
        public virtual bool TryTransition(TWith with)
        {
            var stateType = CurrentState.GetType();
            do
            {
                if (!transitions.ContainsKey(stateType))
                    continue;

                foreach (var transition in transitions[stateType])
                {
                    var attempt = transition.TransitionAttemptBuilder.CreateTransitionAttempt(this, CurrentState, with);

                    if (!transition.CanTransition(attempt))
                        continue;

                    CurrentState = transition.DoTransition(attempt);
                    return true;
                }
            }
            while ((stateType = stateType.GetNextBaseType<TStates>()) != null);

            return false;
        }

        /// <summary>
        /// Produces or loads the transitions for the given assemblies ordered by type.
        /// </summary>
        private Dictionary<Type, TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>[]> buildStateMachine(params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            if (assemblies.Length == 0)
                throw new ArgumentException("There must be at least one assembly to look for Transitions in!");

            var assemblySet = new HashSet<Assembly>(assemblies);

            if (!knownAssemblies.ContainsKey(assemblySet))
            {
                var transitionsTypes = collectMatchingTransitions(assemblies);

                knownAssemblies.Add(assemblySet, transitionsTypes.GroupBy(entry => entry.StateInType).ToDictionary(g => g.Key, g => g.ToArray()));
            }

            return knownAssemblies[assemblySet];
        }

        /// <summary>
        /// Finds all <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/> derivatives in the assemblies that match this state machine.
        /// </summary>
        private IEnumerable<TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>> collectMatchingTransitions(Assembly[] assemblies)
        {
            return assemblies.SelectMany(assembly =>
                // Collect all Transitions and their definitions from all assemblies
                assembly.GetTypes().Select(type =>
                    {
                        var isTransition = type.DerivesFrom(typeof(Transition<,,,,,>), out var transitionDef);
                        return new { IsTransition = isTransition, ConcreteType = type, TransitionType = transitionDef };
                    }))
                .Where(r => r.IsTransition)
                // Don't want any incomplete types
                .Where(r => !r.ConcreteType.IsAbstract)
                // Filter matching Transitions
                // Everything except for TMachine is checked by the compiler
                // Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}
                .Where(r => r.TransitionType.GetGenericArguments()[0].DerivesFromOrIs(thisType))
                // Filter out transitions without parameterless constructors
                .Where(r => r.ConcreteType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Any(ctor => ctor.GetParameters().Length == 0))
                .Select(r => new TransitionEntry<StateMachine<TStates, TWith>, TStates, TWith>(r.ConcreteType, r.TransitionType));
        }

        /// <summary>
        /// Compares <see cref="HashSet{T}"/>s by the elements they contain.
        /// </summary>
        private sealed class HashSetEqualityComparer<T> : IEqualityComparer<HashSet<T>>
        {
            public bool Equals(HashSet<T> x, HashSet<T> y)
            {
                return x.SetEquals(y);
            }

            public int GetHashCode(HashSet<T> obj)
            {
                return obj.Select(item => item.GetHashCode())
                    .Aggregate((acc, hash) => unchecked(acc + hash));
            }
        }
    }
}