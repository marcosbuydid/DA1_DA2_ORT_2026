using ObserverPattern.ConcertTicketSystem.Observer;

namespace ObserverPattern.ConcertTicketSystem.ConcreteObservers
{
    public class User : IObserver
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{Name} receive notification: {message}");
        }
    }
}
