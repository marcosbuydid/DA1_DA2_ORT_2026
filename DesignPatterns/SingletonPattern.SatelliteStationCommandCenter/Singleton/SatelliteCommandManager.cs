namespace SingletonPattern.SatelliteStationCommandCenter.Singleton
{
    public sealed class SatelliteCommandManager
    {
        private static readonly SatelliteCommandManager _instance = new SatelliteCommandManager();

        public static SatelliteCommandManager Instance
        {
            get { return _instance; }
        }

        private SatelliteCommandManager()
        {
            //prevent external instantiation
        }

        public void SendCommand(string satellite)
        {
            Console.WriteLine($"Command sent to {satellite}");
        }

        private readonly Queue<string> commandQueue = new Queue<string>();

        public void EnqueueCommand(string command)
        {
            commandQueue.Enqueue(command);
            Console.WriteLine($"Command queued: {command}");
        }

        public void TransmitNextCommand()
        {
            if (commandQueue.Count > 0)
            {
                string command = commandQueue.Dequeue();
                Console.WriteLine($"Transmitting command to satellite: {command}");
            }
        }
    }
}
