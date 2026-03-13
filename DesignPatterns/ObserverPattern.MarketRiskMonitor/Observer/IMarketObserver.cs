using ObserverPattern.MarketRiskMonitor.DomainModel;

namespace ObserverPattern.MarketRiskMonitor.Observer
{
    public interface IMarketObserver
    {
        void Update(MarketEvent marketEvent);
    }
}
