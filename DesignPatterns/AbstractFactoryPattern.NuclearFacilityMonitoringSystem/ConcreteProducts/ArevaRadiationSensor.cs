
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractProducts;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteProducts
{
    internal class ArevaRadiationSensor : IRadiationSensor
    {
        public void DetectRadiation()
        {
            Console.WriteLine("Areva radiation sensor analyzing spectrum.");
        }
    }
}
