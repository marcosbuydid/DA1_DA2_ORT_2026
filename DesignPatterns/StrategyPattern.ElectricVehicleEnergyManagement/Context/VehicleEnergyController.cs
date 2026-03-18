
using StrategyPattern.ElectricVehicleEnergyManagement.Strategy;

namespace StrategyPattern.ElectricVehicleEnergyManagement.Context
{
    public class VehicleEnergyController
    {
        private IEnergyManagementStrategy _strategy;

        public VehicleEnergyController(IEnergyManagementStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IEnergyManagementStrategy strategy)
        {
            _strategy = strategy;
        }

        public void ExecuteStrategy()
        {
            _strategy.ManageEnergy();
        }
    }
}
