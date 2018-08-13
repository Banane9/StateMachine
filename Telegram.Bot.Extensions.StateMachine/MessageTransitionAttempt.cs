using StateMachine;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.StateMachine
{
    /// <summary>
    /// Represents a slightly less generic transition attempt.
    /// Fully usable, but can be derived from for brevity's sake.
    /// </summary>
    /// <typeparam name="TMachine">The type of the <see cref="MessageStateMachine{TStates}"/> that's used with
    /// the <see cref="MessageTransitionAttempt{TMachine, TStates, TStateIn}"/>.</typeparam>
    /// <typeparam name="TStates">The base type of the states used by the machine.</typeparam>
    /// <typeparam name="TStateIn">The base type of the state that the machine must be in to use this transition.</typeparam>
    public class MessageTransitionAttempt<TMachine, TStates, TStateIn> : TransitionAttempt<TMachine, TStates, TStateIn, Message>
        where TMachine : MessageStateMachine<TStates>
        where TStates : MachineState
        where TStateIn : TStates
    { }
}