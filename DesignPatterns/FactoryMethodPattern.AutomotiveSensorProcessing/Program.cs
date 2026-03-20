using FactoryMethodPattern.AutomotiveSensorProcessing.ConcreteCreators;
using FactoryMethodPattern.AutomotiveSensorProcessing.Creator;

namespace FactoryMethodPattern.AutomotiveSensorProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SensorProcessorCreator creator;

            creator = new LidarProcessorCreator();
            creator.ExecuteProcessing();

            creator = new RadarProcessorCreator();
            creator.ExecuteProcessing();

            creator = new CameraProcessorCreator();
            creator.ExecuteProcessing();
        }
    }
}
