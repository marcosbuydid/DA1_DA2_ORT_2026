using BuilderPattern.AutonomousDroneMission.Builder;
using BuilderPattern.AutonomousDroneMission.ConcreteBuilder;
using BuilderPattern.AutonomousDroneMission.Director;
using BuilderPattern.AutonomousDroneMission.Product;

namespace BuilderPattern.AutonomousDroneMission
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MissionPlanner missionPlannerOne = new MissionPlanner();

            IMissionBuilder surveillanceMissionBuilder = new SurveillanceMissionBuilder();

            Mission surveillanceMission = missionPlannerOne.CreateMission(surveillanceMissionBuilder);

            surveillanceMission.Display();


            MissionPlanner missionPlannerTwo = new MissionPlanner();

            IMissionBuilder deliveryMissionBuilder = new DeliveryMissionBuilder();

            Mission deliveryMission = missionPlannerTwo.CreateMission(deliveryMissionBuilder);

            deliveryMission.Display();
        }
    }
}
