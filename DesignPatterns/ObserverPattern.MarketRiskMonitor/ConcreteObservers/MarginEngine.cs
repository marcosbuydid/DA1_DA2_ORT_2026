using ObserverPattern.MarketRiskMonitor.DomainModel;
using ObserverPattern.MarketRiskMonitor.Observer;

namespace ObserverPattern.MarketRiskMonitor.ConcreteObservers
{
    public class MarginEngine : IMarketObserver
    {
        public void Update(MarketEvent marketEvent)
        {
            Console.WriteLine($"[Margin] Updating collateral for {marketEvent.Symbol}");
        }
    }
}
