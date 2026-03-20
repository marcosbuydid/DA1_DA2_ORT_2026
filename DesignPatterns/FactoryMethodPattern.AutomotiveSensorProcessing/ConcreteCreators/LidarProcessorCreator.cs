using FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteProducts;
using FactoryMethodPattern.AutomotiveSensorProcessing.Creator;
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteCreators
{
    public class LidarProcessorCreator : SensorProcessorCreator
    {
        public override ISensorProcessor CreateProcessor()
        {
            return new LidarProcessor();
        }
    }
}
