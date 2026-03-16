
using ProxyPattern.IndustrialRobotSafetyDelegate.Subject;

namespace ProxyPattern.IndustrialRobotSafetyDelegate.RealSubject
{
    public class RobotController : IRobotController
    {
        public void ExecuteTask(string task)
        {
            Console.WriteLine($"Robot executing task: {task}");
        }
    }
}
