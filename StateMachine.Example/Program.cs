using System;

namespace StateMachine.Example
{
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

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }
    }
}