using System;
using System.Reflection;
using StateMachine;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.StateMachine
{
    /// <summary>
    /// Represents the abstract base class for state machines that use the <see cref="Message"/>s from any kind of <see cref="Types.Chat"/>.
    /// </summary>
    /// <typeparam name="TStates">The base type of the states used by this machine.</typeparam>
    public abstract class MessageStateMachine<TStates> : StateMachine<TStates, Message>
        where TStates : MachineState
    {
        /// <summary>
        /// Gets the <see cref="Types.Chat"/> that this state machine belongs to.
        /// </summary>
        public Chat Chat { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="MessageStateMachine{TStates}"/> class,
        /// looking for matching <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the
        /// <see cref="Assembly"/> where the deriving class is defined.
        /// </summary>
        /// <param name="startState">The state that the state machine starts in.</param>
        protected MessageStateMachine(Chat chat, TStates startState)
                    : base(startState)
        {
            Chat = chat;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StateMachine{TStates, TWith}"/> class, looking for matching
        /// <see cref="Transition{TMachine, TStates, TStateIn, TWith, TStateOut, TTransitionAttempt}"/>s in the given <see cref="Assembly"/>s.
        /// </summary>
        /// <param name="startState">The state that the state machine starts in.</param>
        /// <param name="assemblies">The <see cref="Assembly"/>s to look for matching Transitions in.</param>
        protected MessageStateMachine(Chat chat, TStates startState, params Assembly[] assemblies)
            : base(startState, assemblies)
        {
            Chat = chat;
        }

        /// <summary>
        /// Tries transitioning from the current state with the given input.
        /// <see cref="Message"/> must be from the chat that this state machine is for.
        /// Tries all transitions for the current type of the state, up to the base state type of the machine.
        /// </summary>
        /// <param name="with">The input <see cref="Message"/> to try and transition with.
        /// Must be from the <see cref="Types.Chat"/> that this state machine is for.</param>
        /// <returns>Whether a transition was successfully executed or not.</returns>
        public override bool TryTransition(Message with)
        {
            if (with.Chat.Id != Chat.Id)
                throw new ArgumentException("Message must be from the chat that this state machine is for!", nameof(with));

            // Update the Chat if it's migrated to supergroup... once those messages actually get to a bot?

            return base.TryTransition(with);
        }
    }
}