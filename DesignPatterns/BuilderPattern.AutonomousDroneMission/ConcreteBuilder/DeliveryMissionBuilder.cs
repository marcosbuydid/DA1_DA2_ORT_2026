
using BuilderPattern.AutonomousDroneMission.BuilderInterface;
using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission.ConcreteBuilder
{
    public class DeliveryMissionBuilder : IMissionBuilder
    {
        private Mission mission = new Mission();

        public void ConfigureName()
        {
            mission.Name = "Delivery Mission";
        }

        public void ConfigureNavigation()
        {
            mission.NavigationPlan = "GPS delivery route";
        }

        public void ConfigureSensors()
        {
            mission.SensorConfiguration = "Obstacle avoidance lidar";
        }

        public void ConfigureCommunication()
        {
            mission.CommunicationProtocol = "Standard telemetry link";
        }

        public void ConfigureSafety()
        {
            mission.SafetyPolicy = "Auto landing if battery < 20%";
        }

        public void ConfigurePayload()
        {
            mission.Payload = "Package delivery compartment";
        }

        public Mission GetMission()
        {
            return mission;
        }
    }
}
