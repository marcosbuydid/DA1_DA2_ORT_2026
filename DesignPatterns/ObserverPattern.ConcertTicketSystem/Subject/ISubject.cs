using ObserverPattern.ConcertTicketSystem.Observer;

namespace ObserverPattern.ConcertTicketSystem.Subject
{
    //interface that manages subscriptions
    public interface ISubject
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void Notify();
    }
}
