
using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class ChargingState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            Console.WriteLine("State: Charging - Delivering maximum current...");

            //simulate a random fault during charging
            bool faultDetected = new Random().Next(0, 2) == 0;

            if (faultDetected)
            {
                controller.SetState(new FaultState());
                return;
            }

            controller.SetState(new TaperingState());
        }
    }
}
