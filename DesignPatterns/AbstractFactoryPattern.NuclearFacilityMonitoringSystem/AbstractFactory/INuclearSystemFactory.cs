
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractFactory
{
    public interface INuclearSystemFactory
    {
        IRadiationSensor CreateRadiationSensor();
        ICoolingController CreateReactorCoolantSystem();
        IShutdownManager CreateReactorProtectionSystem();
    }
}
