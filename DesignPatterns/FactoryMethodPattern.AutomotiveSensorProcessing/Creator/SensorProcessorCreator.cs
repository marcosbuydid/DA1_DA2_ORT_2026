
using FactoryMethodPattern.AutomotiveSensorProcessing.Product;

namespace FactoryMethodPattern.AutomotiveSensorProcessing.Creator
{
    public abstract class SensorProcessorCreator
    {
        //factory method
        public abstract ISensorProcessor CreateProcessor();

        //template method (optional)
        public void ExecuteProcessing()
        {
            ISensorProcessor sensorProcessor = CreateProcessor();
            sensorProcessor.Process();
        }
    }
}
