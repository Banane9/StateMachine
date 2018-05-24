using System;
using System.Collections.Generic;
using CompiledFilters;

namespace StateMachine
{
    // Check inheritance of the state type to allow something like ICancelable for commands
    public abstract class Transition<TStates, TWith>
    {
        public abstract Filter<TransitionAttempt<TStates, TWith>> TransitionFilter { get; }

        public bool CanTransition(TransitionAttempt<TStates, TWith> transitionAttempt)
                    => TransitionFilter.GetCompiledFilter()(transitionAttempt);

        public bool CanTransition(TStates state, TWith with)
            => TransitionFilter.GetCompiledFilter()(new TransitionAttempt<TStates, TWith>(state, with));

        public abstract TStates DoTransition(TransitionAttempt<TStates, TWith> transitionAttempt);

        public TStates DoTransition(TStates state, TWith with)
            => DoTransition(new TransitionAttempt<TStates, TWith>(state, with));
    }
}