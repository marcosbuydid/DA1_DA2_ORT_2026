using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission.Builder
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
