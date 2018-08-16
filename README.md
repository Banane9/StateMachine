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
public abstract class MyTransition<TStateIn, TStateOut> : Transition<MyStateMachine, ExampleStates, TStateIn, string, TStateOut>
    where TStateIn : ExampleStates
    where TStateOut : ExampleStates
{ }

public class StartTransition : MyTransition<StartState, OtherState>
{
    public override bool CanTransition(ExampleStateMachine machine, StartState state, string with)
    {
        // using int.TryParse as a check for "is this parseable?"
        return int.TryParse(with, out var _);
    }

    public override OtherState DoTransition(ExampleStateMachine machine, StartState state, string with)
    {
        attempt.Machine.Print("You wrote something that's a number! Now going into the other state.");

        return new OtherState(int.Parse(with));
    }
}

public class OtherTransition : MyTransition<OtherState, StartState>
{
    public override bool CanTransition(ExampleStateMachine machine, OtherState state, string with)
    {
        return true;
    }

    public override StartState DoTransition(ExampleStateMachine machine, OtherState state, string with)
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
        var sm = new MyStateMachine();
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
