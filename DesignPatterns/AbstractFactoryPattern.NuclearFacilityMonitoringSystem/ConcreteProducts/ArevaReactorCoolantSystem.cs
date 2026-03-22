using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    public class ArevaReactorCoolantSystem : ICoolingController
    {
        public void RegulateTemperature()
        {
            Console.WriteLine("Areva cooling system stabilizing reactor core temperature.");
        }
    }
}
