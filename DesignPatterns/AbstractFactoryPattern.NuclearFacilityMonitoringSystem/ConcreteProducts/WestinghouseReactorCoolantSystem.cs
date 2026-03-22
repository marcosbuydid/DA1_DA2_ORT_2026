
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    public class WestinghouseReactorCoolantSystem : ICoolingController
    {
        public void RegulateTemperature()
        {
            Console.WriteLine("Westinghouse cooling system regulating reactor temperature.");
        }
    }
}
