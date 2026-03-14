using SingletonPattern.SatelliteStationCommandCenter.Singleton;

namespace SingletonPattern.SatelliteStationCommandCenter.Subsystems
{
    public class PowerManagementSystem
    {
        public void EnterLowPowerMode()
        {
            SatelliteCommandManager.Instance.EnqueueCommand("Enter Low Power Mode");
        }
    }
}
