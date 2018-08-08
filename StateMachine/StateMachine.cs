using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StateMachine
{
    public abstract class StateMachine<TStates, TWith> : IStateMachine<TStates, TWith>
        where TStates : MachineState
    {
        private static readonly Type statesType = typeof(TStates);
        private static readonly Type transitionType = typeof(Transition<,,,,>);
        private static readonly Type withType = typeof(TWith);
        private readonly Type thisType;

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class,
        /// looking for matching <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut}"/>s in the
        /// <see cref="Assembly"/> where the deriving class is defined.
        /// </summary>
        protected StateMachine()
        {
            thisType = GetType();
            buildStateMachine(thisType.Assembly);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class, looking for matching
        /// <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut}"/>s in the given <see cref="Assembly"/>s.
        /// </summary>
        /// <param name="assemblies">The <see cref="Assembly"/>s to look for matching Transitions in.</param>
        protected StateMachine(params Assembly[] assemblies)
        {
            buildStateMachine(assemblies);
        }

        private void buildStateMachine(params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            if (assemblies.Length == 0)
                throw new ArgumentException("There must be at least one assemblies to look for Transitions in!");

            var transitions = collectMatchingTransitions(assemblies);
        }

        private Tuple<Type, Type>[] collectMatchingTransitions(Assembly[] assemblies)
        {
            return assemblies.SelectMany(assembly =>
                // Collect all Transitions and their definitions from all assemblies
                assembly.GetTypes().Select(type =>
                    {
                        var isTransition = type.DerivesFrom(transitionType, out var transitionDef);
                        return new { IsTransition = isTransition, ConcreteType = type, TransitionType = transitionDef };
                    }))
                .Where(r => r.IsTransition)
                // Don't want any incomplete types
                .Where(r => !r.ConcreteType.IsAbstract)
                // Filter matching Transitions
                // Transition{TMachine, TStates, TStateIn, TWith, TStateOut}
                // Everything except for TMachine is checked by the compiler
                .Where(r => r.TransitionType.GetGenericArguments()[0].DerivesFromOrIs(thisType))
                .Select(r => new Tuple<Type, Type>(r.ConcreteType, r.TransitionType))
                .ToArray();
        }
    }
}