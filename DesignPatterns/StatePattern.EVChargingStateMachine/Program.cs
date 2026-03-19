using StatePattern.EVChargingStateMachine.ConcreteStates;
using StatePattern.EVChargingStateMachine.Context;

namespace StatePattern.EVChargingStateMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //The client does not choose behavior, only triggers processing
            EVChargingController controller = new EVChargingController();

            Console.WriteLine("Charging cycle example:");
            Console.WriteLine();

            for (int i = 0; i < 5; i++)
            {
                controller.Process();

                if (controller.chargingState is FaultState)
                {
                    Console.WriteLine("State: Fault detected. Charger disconnected");
                    break;
                }
            }
        }
    }
}
