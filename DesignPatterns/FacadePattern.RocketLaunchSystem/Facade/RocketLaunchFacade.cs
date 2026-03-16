
using FacadePattern.RocketLaunchSystem.Subsystems;

namespace FacadePattern.RocketLaunchSystem.Facade
{
    public class RocketLaunchFacade
    {
        private PropulsionSubsystem propulsionSubsystem = new PropulsionSubsystem();
        private GuidanceSubsystem guidanceSubsystem = new GuidanceSubsystem();
        private TelemetrySubsystem telemetrySubsystem = new TelemetrySubsystem();

        public void PrepareLaunch()
        {
            Console.WriteLine("Rocket launch preparation started.");

            Console.WriteLine();

            propulsionSubsystem.ChillEngines();
            propulsionSubsystem.PressurizeFuel();

            guidanceSubsystem.AlignIMU();
            guidanceSubsystem.LoadTrajectory();

            telemetrySubsystem.InitializeLink();

            Console.WriteLine();

            Console.WriteLine("Rocket is ready for launch.");
        }
    }
}
