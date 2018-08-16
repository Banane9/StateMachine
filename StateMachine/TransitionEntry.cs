using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StateMachine
{
    internal sealed class TransitionEntry<TMachine, TStates, TWith>
            where TMachine : StateMachine<TStates, TWith>
            where TStates : MachineState
    {
        private static readonly Type objType = typeof(object);

        private readonly Func<object, TMachine, TStates, TWith, bool> canTransition;
        private readonly Func<object, TMachine, TStates, TWith, TStates> doTransition;
        private readonly object transition;
        public Type ConcreteTransition { get; }

        public Type StateInType { get; }
        public Type StateOutType { get; }
        public Type TransitionDefinition { get; }

        public TransitionEntry(Type concreteTransition, Type transitionDefinition)
        {
            ConcreteTransition = concreteTransition;
            TransitionDefinition = transitionDefinition;

            transition = Activator.CreateInstance(concreteTransition);

            // Transition{TMachine, TStates, TStateIn, TWith, TStateOut}
            var genericArguments = transitionDefinition.GetGenericArguments();
            StateInType = genericArguments[2];

            var transitionParam = Expression.Parameter(objType);
            var transitionCast = Expression.Convert(transitionParam, concreteTransition);

            var machineParam = Expression.Parameter(typeof(TMachine));

            var stateParam = Expression.Parameter(typeof(TStates));
            var stateCast = Expression.Convert(stateParam, StateInType);

            var withParam = Expression.Parameter(typeof(TWith));

            canTransition = Expression.Lambda<Func<object, TMachine, TStates, TWith, bool>>(
                Expression.Call(transitionCast,
                    concreteTransition.GetMethod("CanTransition"),
                    machineParam,
                    stateCast,
                    withParam),
                transitionParam,
                machineParam,
                stateParam,
                withParam).Compile();

            var doTransitionMethod = concreteTransition.GetMethod("DoTransition");
            doTransition = Expression.Lambda<Func<object, TMachine, TStates, TWith, TStates>>(
                Expression.Convert(
                    Expression.Call(transitionCast,
                        doTransitionMethod,
                        machineParam,
                        stateCast,
                        withParam),
                    typeof(TStates)),
                transitionParam,
                machineParam,
                stateParam,
                withParam).Compile();

            StateOutType = doTransitionMethod.ReturnType;
        }

        public bool CanTransition(TMachine machine, TStates state, TWith with)
        {
            return canTransition(transition, machine, state, with);
        }

        public TStates DoTransition(TMachine machine, TStates state, TWith with)
        {
            return doTransition(transition, machine, state, with);
        }
    }
}