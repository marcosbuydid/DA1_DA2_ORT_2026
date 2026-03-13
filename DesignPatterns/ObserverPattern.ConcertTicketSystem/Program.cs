using ObserverPattern.ConcertTicketSystem.ConcreteObservers;
using ObserverPattern.ConcertTicketSystem.ConcreteSubject;

namespace ObserverPattern.ConcertTicketSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConcertTicketManager concertTicketManager = new ConcertTicketManager("Ibiza 2026 Electronic Concert");

            User userOne = new User("Silvina");
            User userTwo = new User("Marcos");

            concertTicketManager.Subscribe(userOne);
            concertTicketManager.Subscribe(userTwo);

            concertTicketManager.ReleaseTickets(101124);

            Console.ReadLine();
        }
    }
}
