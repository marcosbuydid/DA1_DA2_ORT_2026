
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteProducts
{
    public class RadarProcessor : ISensorProcessor
    {
        public void Process()
        {
            Console.WriteLine("Processing radar signals");
        }
    }
}
