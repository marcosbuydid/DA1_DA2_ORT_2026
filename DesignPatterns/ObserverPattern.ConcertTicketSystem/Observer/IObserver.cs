namespace ObserverPattern.ConcertTicketSystem.Observer
{
    public interface IObserver
    {
        //define the method to execute when new tickets are available
        void Update(string message);
    }
}
