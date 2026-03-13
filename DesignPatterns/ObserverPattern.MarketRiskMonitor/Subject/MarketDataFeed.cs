using ObserverPattern.MarketRiskMonitor.DomainModel;
using ObserverPattern.MarketRiskMonitor.Observer;

namespace ObserverPattern.MarketRiskMonitor.Subject
{
    public class MarketDataFeed
    {
        private readonly List<IMarketObserver> observers = new List<IMarketObserver>();

        public void Attach(IMarketObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IMarketObserver observer)
        {
            observers.Remove(observer);
        }

        public void PublishPrice(string symbol, decimal price)
        {
            MarketEvent marketEvent = new MarketEvent(symbol, price);
            Notify(marketEvent);
        }

        private void Notify(MarketEvent marketEvent)
        {
            foreach (var observer in observers)
            {
                observer.Update(marketEvent);
            }
        }
    }
}
