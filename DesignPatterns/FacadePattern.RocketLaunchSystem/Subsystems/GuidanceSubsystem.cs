namespace FacadePattern.RocketLaunchSystem.Subsystems
{
    public class GuidanceSubsystem
    {
        public void AlignIMU()
        {
            Console.WriteLine("Inertial measurement unit aligned.");
        }

        public void LoadTrajectory()
        {
            Console.WriteLine("Flight trajectory loaded.");
        }
    }
}
