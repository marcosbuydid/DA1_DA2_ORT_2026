using SingletonPattern.SatelliteStationCommandCenter.Singleton;

namespace SingletonPattern.SatelliteStationCommandCenter.Subsystems
{
    public class NavigationSubsystem
    {
        public void AdjustOrbit()
        {
            SatelliteCommandManager.Instance.EnqueueCommand("Adjust Orbit");
        }
    }
}
