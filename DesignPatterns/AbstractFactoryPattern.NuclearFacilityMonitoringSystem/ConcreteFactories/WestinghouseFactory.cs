
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractFactory;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteFactories
{
    public class WestinghouseFactory : INuclearSystemFactory
    {
        public IRadiationSensor CreateRadiationSensor()
        {
            return new WestinghouseRadiationSensor();
        }

        public ICoolingController CreateReactorCoolantSystem()
        {
            return new WestinghouseReactorCoolantSystem();
        }

        public IShutdownManager CreateReactorProtectionSystem()
        {
            return new WestinghouseReactorProtectionSystem();
        }
    }
}
