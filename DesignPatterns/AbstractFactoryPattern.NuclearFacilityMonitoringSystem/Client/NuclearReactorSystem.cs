
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractFactory;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.Client
{
    public class NuclearReactorSystem
    {
        private readonly IRadiationSensor _radiationSensor;
        private readonly ICoolingController _coolantSystem;
        private readonly IShutdownManager _shutdownSystem;

        public NuclearReactorSystem(INuclearSystemFactory factory)
        {
            _radiationSensor = factory.CreateRadiationSensor();
            _coolantSystem = factory.CreateReactorCoolantSystem();
            _shutdownSystem = factory.CreateReactorProtectionSystem();
        }

        public void Run()
        {
            _radiationSensor.DetectRadiation();
            _coolantSystem.RegulateTemperature();

            //simulate emergency scenario in the nuclear facility
            Console.WriteLine("Radiation spike detected outside reactor A!");
            _shutdownSystem.ActivateEmergencyShutdown();
        }
    }
}
