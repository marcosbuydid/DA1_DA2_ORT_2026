using ObserverPattern.ConcertTicketSystem.Observer;
using ObserverPattern.ConcertTicketSystem.Subject;

namespace ObserverPattern.ConcertTicketSystem.ConcreteSubject
{
    public class ConcertTicketManager : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();
        private int availableTickets = 0;
        private string concertName = "";

        public ConcertTicketManager(string eventName) 
        { 
            concertName = eventName;
        }
        
        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify()
        {
            foreach(IObserver observer in observers)
            {
                observer.Update($"There are {availableTickets} tickets available for {concertName}");
            }
        }

        public void ReleaseTickets(int quantity)
        {
            availableTickets += quantity;
            Console.WriteLine($"{quantity} tickets have been released for {concertName}");
            Notify();
        }
    }
}
