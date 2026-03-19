
using StatePattern.EVChargingStateMachine.Context;

namespace StatePattern.EVChargingStateMachine.State
{
    public interface IChargingState
    {
        void HandleCharging(EVChargingController controller);
    }
}
