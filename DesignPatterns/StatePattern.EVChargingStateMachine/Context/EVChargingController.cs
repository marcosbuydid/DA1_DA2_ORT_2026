using StatePattern.EVChargingStateMachine.ConcreteStates;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.Context
{
    public class EVChargingController
    {
        public IChargingState chargingState;

        public EVChargingController()
        {
            chargingState = new IdleState();
        }

        public void SetState(IChargingState state)
        {
            chargingState = state;
        }

        public void Process()
        {
            chargingState.HandleCharging(this);
        }
    }
}
