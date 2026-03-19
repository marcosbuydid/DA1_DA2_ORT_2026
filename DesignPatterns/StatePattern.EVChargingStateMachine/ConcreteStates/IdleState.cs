
using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class IdleState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            Console.WriteLine("State: Idle - Waiting for plug-in...");

            //transition
            controller.SetState(new HandshakeState());
        }
    }
}
