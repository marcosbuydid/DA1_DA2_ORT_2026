
using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class FullChargeState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            Console.WriteLine("State: Full Charge - Charger can be disconnected...");

            //cycle complete
            controller.SetState(new IdleState());
        }
    }
}
