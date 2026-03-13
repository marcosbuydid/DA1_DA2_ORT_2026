using ObserverPattern.MarketRiskMonitor.ConcreteObservers;
using ObserverPattern.MarketRiskMonitor.Subject;

namespace ObserverPattern.MarketRiskMonitor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MarketDataFeed marketDataFeed = new MarketDataFeed();

            ValueAtRiskEngine valueAtRiskEngine = new ValueAtRiskEngine();
            MarginEngine marginEngine = new MarginEngine();
            VolatilityAlertEngine volatilityAlertEngine = new VolatilityAlertEngine();

            marketDataFeed.Attach(valueAtRiskEngine);
            marketDataFeed.Attach(marginEngine);
            marketDataFeed.Attach(volatilityAlertEngine);

            marketDataFeed.PublishPrice("AAPL", 185.23m);
            marketDataFeed.PublishPrice("TSLA", 1045.10m);
        }
    }
}
