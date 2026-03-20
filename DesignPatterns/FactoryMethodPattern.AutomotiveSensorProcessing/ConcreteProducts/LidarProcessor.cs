
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteProducts
{
    public class LidarProcessor : ISensorProcessor
    {
        public void Process()
        {
            Console.WriteLine("Processing LiDAR point cloud");
        }
    }
}
