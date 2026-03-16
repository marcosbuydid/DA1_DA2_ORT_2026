using ProxyPattern.IndustrialRobotSafetyDelegate.Proxy;

namespace ProxyPattern.IndustrialRobotSafetyDelegate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RobotControllerProxy controllerProxy = new RobotControllerProxy();

            controllerProxy.ExecuteTask("Welding joint AFR56HK");

            controllerProxy.SafetyZoneClear = false;

            controllerProxy.ExecuteTask("Cutting metal plate number 3443673");
        }
    }
}
