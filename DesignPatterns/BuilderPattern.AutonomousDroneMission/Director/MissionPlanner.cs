using BuilderPattern.AutonomousDroneMission.Builder;
using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission.Director
{
    public class MissionPlanner ()
    {
        public Mission CreateMission(IMissionBuilder missionBuilder)
        {
            missionBuilder.ConfigureName();
            missionBuilder.ConfigureNavigation();
            missionBuilder.ConfigureSensors();
            missionBuilder.ConfigureCommunication();
            missionBuilder.ConfigureSafety();
            missionBuilder.ConfigurePayload();

            return missionBuilder.GetMission();
        }
    }
}
