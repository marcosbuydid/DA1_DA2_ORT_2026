using StrategyPattern.ElectricVehicleEnergyManagement.Strategy;

namespace StrategyPattern.ElectricVehicleEnergyManagement.ConcreteStrategies
{
    public class RegenerativeModeStrategy : IEnergyManagementStrategy
    {
        public void ManageEnergy()
        {
            Console.WriteLine("Maximizing regenerative braking efficiency.");
        }
    }
}
