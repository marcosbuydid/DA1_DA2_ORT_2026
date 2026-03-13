using ObserverPattern.MarketRiskMonitor.DomainModel;
using ObserverPattern.MarketRiskMonitor.Observer;

namespace ObserverPattern.MarketRiskMonitor.ConcreteObservers
{
    public class VolatilityAlertEngine : IMarketObserver
    {
        public void Update(MarketEvent marketEvent)
        {
            if (marketEvent.Price > 1000)
            {
                Console.WriteLine($"[Alert] High price movement detected for {marketEvent.Symbol}");
            }
        }
    }
}
