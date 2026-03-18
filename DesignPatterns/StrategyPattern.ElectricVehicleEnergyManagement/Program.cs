using StrategyPattern.ElectricVehicleEnergyManagement.ConcreteStrategies;
using StrategyPattern.ElectricVehicleEnergyManagement.Context;

namespace StrategyPattern.ElectricVehicleEnergyManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VehicleEnergyController vehicleEnergyController = new VehicleEnergyController(new RangeOptimizationStrategy());

            Console.WriteLine("Initial Mode:");
            vehicleEnergyController.ExecuteStrategy();

            //simulating system conditions
            bool lowBattery = true;
            bool driverWantsPerformance = true;
            bool brakingDetected = true;

            Console.WriteLine("\n--- System Monitoring Loop ---\n");

            if (lowBattery)
            {
                Console.WriteLine("Event: Low battery detected --- Switching to Range Optimization");
                vehicleEnergyController.SetStrategy(new RangeOptimizationStrategy());
                vehicleEnergyController.ExecuteStrategy();
            }

            if (driverWantsPerformance)
            {
                Console.WriteLine("\nEvent: Driver selected SPORT mode --- Switching to Performance");
                vehicleEnergyController.SetStrategy(new PerformanceModeStrategy());
                vehicleEnergyController.ExecuteStrategy();
            }

            if (brakingDetected)
            {
                Console.WriteLine("\nEvent: Braking detected --- Switching to Regenerative Mode");
                vehicleEnergyController.SetStrategy(new RegenerativeModeStrategy());
                vehicleEnergyController.ExecuteStrategy();
            }
        }
    }
}
