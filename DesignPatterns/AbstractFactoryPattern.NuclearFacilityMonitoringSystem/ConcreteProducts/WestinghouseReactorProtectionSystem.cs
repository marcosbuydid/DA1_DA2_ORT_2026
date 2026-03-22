
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    public class WestinghouseReactorProtectionSystem : IShutdownManager
    {
        public void ActivateEmergencyShutdown()
        {
            Console.WriteLine("Westinghouse reactor protection shutdown initiated!");
        }
    }
}
