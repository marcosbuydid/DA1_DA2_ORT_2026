
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    public class ArevaReactorProtectionSystem : IShutdownManager
    {
        public void ActivateEmergencyShutdown()
        {
            Console.WriteLine("Areva reactor shutdown protection sequence activated!");
        }
    }
}
