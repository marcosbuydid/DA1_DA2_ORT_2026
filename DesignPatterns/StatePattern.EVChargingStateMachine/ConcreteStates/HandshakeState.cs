using StatePattern.EVChargingStateMachine.Context;
using StatePattern.EVChargingStateMachine.State;

namespace StatePattern.EVChargingStateMachine.ConcreteStates
{
    public class HandshakeState : IChargingState
    {
        public void HandleCharging(EVChargingController controller)
        {
            Console.WriteLine("State: Handshake - Verifying charger compatibility...");

            controller.SetState(new ChargingState());
        }
    }
}
