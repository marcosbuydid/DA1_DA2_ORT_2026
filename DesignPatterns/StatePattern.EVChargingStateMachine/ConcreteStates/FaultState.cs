using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class FaultState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            throw new NotImplementedException();
        }
    }
}
