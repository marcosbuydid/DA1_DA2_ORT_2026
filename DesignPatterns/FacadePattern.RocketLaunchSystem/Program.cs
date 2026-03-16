using FacadePattern.RocketLaunchSystem.Facade;

namespace FacadePattern.RocketLaunchSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RocketLaunchFacade rocketLaunchFacade = new RocketLaunchFacade();

            rocketLaunchFacade.PrepareLaunch();
        }
    }
}
