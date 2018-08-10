using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StateMachine
{
    internal sealed class TransitionEntry<TMachine, TStates, TWith>
            where TMachine : StateMachine<TStates, TWith>
            where TStates : MachineState
    {
        private static readonly Dictionary<Type, TransitionAttemptBuilder<TMachine, TStates, TWith>> transitionAttemptBuilders =
            new Dictionary<Type, TransitionAttemptBuilder<TMachine, TStates, TWith>>();

        private readonly Func<object, object, bool> canTransition;
        private readonly Func<object, object, TStates> doTransition;
        private readonly object transition;
        public Type ConcreteTransition { get; }

        public bool HasNextInLineStateOut
        {
            get { return NextInLineStateOutType != null; }
        }

        public Type NextInLineStateOutType { get; }
        public Type StateInType { get; }
        public Type StateOutType { get; }
        public Type TransitionAttempt { get; }
        public TransitionAttemptBuilder<TMachine, TStates, TWith> TransitionAttemptBuilder { get; }
        public Type TransitionDefinition { get; }

        public TransitionEntry(Type concreteTransition, Type transitionDefinition)
        {
            ConcreteTransition = concreteTransition;
            TransitionDefinition = transitionDefinition;

            transition = Activator.CreateInstance(concreteTransition);

            // Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}
            var genericArguments = transitionDefinition.GetGenericArguments();
            StateInType = genericArguments[2];
            TransitionAttempt = genericArguments[5];

            if (!transitionAttemptBuilders.ContainsKey(TransitionAttempt))
                transitionAttemptBuilders.Add(TransitionAttempt, new TransitionAttemptBuilder<TMachine, TStates, TWith>(TransitionAttempt));

            TransitionAttemptBuilder = transitionAttemptBuilders[TransitionAttempt];

            var transitionParam = Expression.Parameter(concreteTransition);
            var transitionCast = Expression.Convert(transitionParam, concreteTransition);

            var attemptParam = Expression.Parameter(TransitionAttempt);
            var attemptCast = Expression.Convert(attemptParam, TransitionAttempt);

            canTransition = Expression.Lambda<Func<object, object, bool>>(
                Expression.Call(transitionCast, concreteTransition.GetMethod("CanTransition"), attemptCast),
                transitionParam,
                attemptParam).Compile();

            var doTransitionMethod = concreteTransition.GetMethod("DoTransition");
            doTransition = Expression.Lambda<Func<object, object, TStates>>(
                Expression.Convert(
                    Expression.Call(transitionCast, doTransitionMethod, attemptCast),
                    typeof(TStates)),
                transitionParam,
                attemptParam).Compile();

            StateOutType = doTransitionMethod.ReturnType;
            NextInLineStateOutType = StateOutType.BaseType != typeof(TStates).BaseType ? StateOutType.BaseType : null;
        }

        public bool CanTransition(object attempt)
        {
            return canTransition(transition, attempt);
        }

        public TStates DoTransition(object attempt)
        {
            return doTransition(transition, attempt);
        }
    }
}