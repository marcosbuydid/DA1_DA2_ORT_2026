using SingletonPattern.SatelliteStationCommandCenter.Singleton;
using SingletonPattern.SatelliteStationCommandCenter.Subsystems;

namespace SingletonPattern.SatelliteStationCommandCenter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var navigationSubSystem = new NavigationSubsystem();
            var powerManagementSystem = new PowerManagementSystem();

            navigationSubSystem.AdjustOrbit();
            powerManagementSystem.EnterLowPowerMode();

            SatelliteCommandManager satelliteCommandManager = SatelliteCommandManager.Instance;

            satelliteCommandManager.TransmitNextCommand();
            satelliteCommandManager.TransmitNextCommand();
        }
    }
}
