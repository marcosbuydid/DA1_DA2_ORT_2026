
using FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteProducts;
using FactoryMethodPattern.AutomotiveSensorProcessing.Creator;
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteCreators
{
    internal class CameraProcessorCreator : SensorProcessorCreator
    {
        public override ISensorProcessor CreateProcessor()
        {
            return new CameraProcessor();
        }
    }
}
