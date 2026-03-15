namespace BuilderPattern.AutonomousDroneMission.Product
{
    public class Mission
    {
        public string Name { get; set; }
        public string NavigationPlan { get; set; }
        public string SensorConfiguration { get; set; }
        public string CommunicationProtocol { get; set; }
        public string SafetyPolicy { get; set; }
        public string Payload { get; set; }

        public Mission() { }

        public Mission(string name,string navigationPlan, string sensorConfiguration,
            string communicationProtocol, string safetyPolicy, string payload)
        {
            Name = name;
            NavigationPlan = navigationPlan;
            SensorConfiguration = sensorConfiguration;
            CommunicationProtocol = communicationProtocol;
            SafetyPolicy = safetyPolicy;
            Payload = payload;
        }

        public void Display()
        {
            Console.WriteLine($"{Name} Configuration");
            Console.WriteLine($"Navigation: {NavigationPlan}");
            Console.WriteLine($"Sensors: {SensorConfiguration}");
            Console.WriteLine($"Communication: {CommunicationProtocol}");
            Console.WriteLine($"Safety: {SafetyPolicy}");
            Console.WriteLine($"Payload: {Payload}");
            Console.WriteLine();
        }
    }
}
