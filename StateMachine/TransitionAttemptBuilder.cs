using System;
using System.Collections.Generic;

namespace StateMachine
{
    internal sealed class TransitionAttemptBuilder<TMachine, TStates, TWith>
            where TMachine : StateMachine<TStates, TWith>
            where TStates : MachineState
    {
        private static readonly Type objType = typeof(object);
        private static readonly Type setterDelegate = typeof(Action<object, object>);
        private readonly Func<object> createTransitionAttempt;
        private readonly Action<object, object> setMachine;
        private readonly Action<object, object> setState;
        private readonly Action<object, object> setWith;

        public TransitionAttemptBuilder(Type transitionAttemptType)
        {
            createTransitionAttempt = transitionAttemptType.MakeConstructor();

            setMachine = transitionAttemptType.GetProperty("Machine").MakePropertySetter();
            setState = transitionAttemptType.GetProperty("State").MakePropertySetter();
            setWith = transitionAttemptType.GetProperty("With").MakePropertySetter();
        }

        public object CreateTransitionAttempt()
        {
            return createTransitionAttempt();
        }

        public object CreateTransitionAttempt(TMachine machine, TStates state, TWith with)
        {
            var attempt = CreateTransitionAttempt();
            SetMachine(attempt, machine);
            SetState(attempt, state);
            SetWith(attempt, with);

            return attempt;
        }

        public void SetMachine(object attempt, TMachine machine)
        {
            setMachine(attempt, machine);
        }

        public void SetState(object attempt, TStates state)
        {
            setState(attempt, state);
        }

        public void SetWith(object attempt, TWith with)
        {
            setWith(attempt, with);
        }
    }
}