using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StateMachine
{
    public abstract class StateMachine<TStates, TWith>
        where TStates : MachineState
    {
        private static readonly Type withType = typeof(TWith);
        private readonly Type thisType;

        private readonly Dictionary<Type, Option[]> transitions;

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class,
        /// looking for matching <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the
        /// <see cref="Assembly"/> where the deriving class is defined.
        /// </summary>
        protected StateMachine()
        {
            thisType = GetType();
            transitions = buildStateMachine(thisType.Assembly);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class, looking for matching
        /// <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the given <see cref="Assembly"/>s.
        /// </summary>
        /// <param name="assemblies">The <see cref="Assembly"/>s to look for matching Transitions in.</param>
        protected StateMachine(params Assembly[] assemblies)
        {
            thisType = GetType();
            transitions = buildStateMachine(assemblies);
        }

        private Dictionary<Type, Option[]> buildStateMachine(params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            if (assemblies.Length == 0)
                throw new ArgumentException("There must be at least one assembly to look for Transitions in!");

            var transitionsTypes = collectMatchingTransitions(assemblies);

            return transitionsTypes.GroupBy(o => o.StateInType).ToDictionary(g => g.Key, g => g.ToArray());
        }

        private IEnumerable<Option> collectMatchingTransitions(Assembly[] assemblies)
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
                .Select(r => new Option(r.ConcreteType, r.TransitionType));
        }

        private sealed class Option
        {
            public Type ConcreteTransition { get; }
            public Type StateInType { get; }
            public Type TransitionAttempt { get; }
            public Type TransitionDefinition { get; }

            public Option(Type concreteTransition, Type transitionDefinition)
            {
                ConcreteTransition = concreteTransition;
                TransitionDefinition = transitionDefinition;

                // Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}
                var genericArguments = transitionDefinition.GetGenericArguments();
                StateInType = genericArguments[2];
                TransitionAttempt = genericArguments[5];
            }
        }
    }
}