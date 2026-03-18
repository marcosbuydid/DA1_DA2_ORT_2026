
using StrategyPattern.ElectricVehicleEnergyManagement.Strategy;

namespace StrategyPattern.ElectricVehicleEnergyManagement.ConcreteStrategies
{
    public class PerformanceModeStrategy : IEnergyManagementStrategy
    {
        public void ManageEnergy()
        {
            Console.WriteLine("Delivering maximum power for performance driving.");
        }
    }
}
