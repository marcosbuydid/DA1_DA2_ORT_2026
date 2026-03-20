
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteProducts
{
    public class CameraProcessor : ISensorProcessor
    {
        public void Process()
        {
            Console.WriteLine("Processing camera frames");
        }
    }
}
