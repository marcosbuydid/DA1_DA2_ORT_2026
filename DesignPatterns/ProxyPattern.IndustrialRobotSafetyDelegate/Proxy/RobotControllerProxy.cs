
using ProxyPattern.IndustrialRobotSafetyDelegate.RealSubject;
using ProxyPattern.IndustrialRobotSafetyDelegate.Subject;

namespace ProxyPattern.IndustrialRobotSafetyDelegate.Proxy
{
    public class RobotControllerProxy : IRobotController
    {
        //Proxy checks safety conditions before delegating.

        private RobotController robot = new RobotController();

        public bool EmergencyStopActivated { get; set; }
        public bool SafetyZoneClear { get; set; } = true;

        public void ExecuteTask(string task)
        {
            if (EmergencyStopActivated)
            {
                Console.WriteLine("Proxy: Emergency stop active. Command rejected.");
                return;
            }

            if (!SafetyZoneClear)
            {
                Console.WriteLine("Proxy: Safety zone not clear. Command rejected.");
                return;
            }

            Console.WriteLine("Proxy: Safety check passed.");
            robot.ExecuteTask(task);
        }
    }
}
