using ObserverPattern.MarketRiskMonitor.DomainModel;
using ObserverPattern.MarketRiskMonitor.Observer;

namespace ObserverPattern.MarketRiskMonitor.ConcreteObservers
{
    public class ValueAtRiskEngine : IMarketObserver
    {
        public void Update(MarketEvent marketEvent)
        {
            Console.WriteLine($"[ValueAtRisk] Recalculating risk for {marketEvent.Symbol} at {marketEvent.Price}");
        }
    }
}
