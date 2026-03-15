using BuilderPattern.AutonomousDroneMission.BuilderInterface;
using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission.ConcreteBuilder
{
    public class SurveillanceMissionBuilder : IMissionBuilder
    {
        private Mission mission = new Mission();

        public void ConfigureName()
        {
            mission.Name = "Surveillance Mission";
        }

        public void ConfigureNavigation()
        {
            mission.NavigationPlan = "Autonomous waypoint patrol";
        }

        public void ConfigureSensors()
        {
            mission.SensorConfiguration = "HD camera + thermal sensor";
        }

        public void ConfigureCommunication()
        {
            mission.CommunicationProtocol = "Encrypted telemetry link";
        }

        public void ConfigureSafety()
        {
            mission.SafetyPolicy = "Return-to-home on signal loss";
        }

        public void ConfigurePayload()
        {
            mission.Payload = "High resolution video recorder";
        }

        public Mission GetMission()
        {
            return mission;
        }
    }
}
