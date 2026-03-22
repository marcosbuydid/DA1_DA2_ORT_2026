using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.AbstractFactory;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.Client;
using AbstractFactoryPattern.NuclearFacilityMonitoringSystem.ConcreteFactories;

namespace AbstractFactoryPattern.NuclearFacilityMonitoringSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            INuclearSystemFactory factory;

            factory = new WestinghouseFactory();
            NuclearReactorSystem system = new NuclearReactorSystem(factory);
            system.Run();

            Console.WriteLine();

            factory = new ArevaFactory();
            system = new NuclearReactorSystem(factory);
            system.Run();
        }
    }
}
