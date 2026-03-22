
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractFactory;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteFactories
{
    public class ArevaFactory : INuclearSystemFactory
    {
        public IRadiationSensor CreateRadiationSensor()
        {
            return new ArevaRadiationSensor();
        }

        public ICoolingController CreateReactorCoolantSystem()
        {
            return new ArevaReactorCoolantSystem();
        }

        public IShutdownManager CreateReactorProtectionSystem()
        {
            return new ArevaReactorProtectionSystem();
        }
    }
}
