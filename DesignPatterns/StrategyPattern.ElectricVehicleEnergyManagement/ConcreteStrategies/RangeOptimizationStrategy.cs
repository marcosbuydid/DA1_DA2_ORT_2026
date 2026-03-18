
using StrategyPattern.ElectricVehicleEnergyManagement.Strategy;

namespace StrategyPattern.ElectricVehicleEnergyManagement.ConcreteStrategies
{
    public class RangeOptimizationStrategy : IEnergyManagementStrategy
    {
        public void ManageEnergy()
        {
            Console.WriteLine("Optimizing energy usage for maximum range.");
        }
    }
}
