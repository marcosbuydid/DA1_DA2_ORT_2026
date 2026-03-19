
using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class TaperingState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            Console.WriteLine("State: Tapering - Reducing current near full charge...");

            //cycle complete
            controller.SetState(new FullChargeState()); 
        }
    }
}
