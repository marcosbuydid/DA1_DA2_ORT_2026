
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    public class WestinghouseRadiationSensor : IRadiationSensor
    {
        public void DetectRadiation()
        {
            Console.WriteLine("Westinghouse sensor detecting radiation levels.");
        }
    }
}
