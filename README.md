# StateMachine
State Machine for stuff.

## Usage

Take a look at the StateMachine.Example folder for more structured set up.

``` CSharp
using StateMachine;

public sealed class MyStateMachine : StateMachine<ExampleStates, string>
{
    public MyStateMachine()
        : base(new StartState())
    { }
    
    public void Print(object o) => Console.WriteLine(o);
}


// Not strictly necessary, only if you have multiple state machines in one assembly
public abstract class ExampleStates : MachineState
{ }

public sealed class StartState : ExampleStates
{ }

public sealed class OtherState : ExampleStates
{
    public int Value { get; }
    
    public OtherState(int value)
    {
        Value = value;
    }
}


// not strictly necessary, but makes the following definitions shorter...
public sealed class MyTransitionAttempt<TStateIn> : TransitionAttempt<MyStateMachine, ExampleStates, TStateIn, string>
    where TStateIn : ExampleStates
{ }

public abstract class MyTransition<TStateIn, TStateOut> : Transition<MyStateMachine, ExampleStates, TStateIn, string, TStateOut, MyTransitionAttempt<TStateIn>>
    where TStateIn : ExampleStates
    where TStateOut : ExampleStates
{ }

public class StartTransition : MyTransition<StartState, OtherState>
{
    public override bool CanTransition(MyTransitionAttempt<StartState> attempt)
    {
        // using int.TryParse as a check for "is this parseable?"
        return int.TryParse(attempt.With, out var _);
    }

    public override OtherState DoTransition(MyTransitionAttempt<StartState> attempt)
    {
        attempt.Machine.Print("You wrote something that's a number! Now going into the other state.");

        return new OtherState(int.Parse(attempt.With));
    }
}

public class OtherTransition : MyTransition<OtherState, StartState>
{
    public override bool CanTransition(MyTransitionAttempt<OtherState> attempt)
    {
        return true;
    }

    public override StartState DoTransition(MyTransitionAttempt<OtherState> attempt)
    {
        attempt.Machine.Print("Doesn't matter what you write, " +
            "I'm just going to give you the number you entered before: " + attempt.State.Value);

        return new StartState();
    }
}


internal class Program
{
    private static void Main(string[] args)
    {
        var sm = new ExampleStateMachine();
        Console.Write("Starting state machine, write 'exit' to stop.");

        string read;
        while ((read = Console.ReadLine()) != "exit")
        {
            sm.TryTransition(read);
        }
    }
}
```

It will automatically load the transitions you defined in the assembly that contains the state machine type you're instantiating,
or you can pass it a `params` aray of assemblies to load.
