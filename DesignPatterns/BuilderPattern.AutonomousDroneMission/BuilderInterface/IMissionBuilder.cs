using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission.BuilderInterface
{
    public interface IMissionBuilder
    {
        void ConfigureName();
        void ConfigureNavigation();
        void ConfigureSensors();
        void ConfigureCommunication();
        void ConfigureSafety();
        void ConfigurePayload();
        Mission GetMission();
    }
}
