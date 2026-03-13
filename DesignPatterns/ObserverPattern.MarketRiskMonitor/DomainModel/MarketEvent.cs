namespace ObserverPattern.MarketRiskMonitor.DomainModel
{
    //it represents the state change information passed during notification
    public class MarketEvent
    {
        public string Symbol { get; }
        public decimal Price { get; }
        public DateTime Timestamp { get; }

        public MarketEvent(string symbol, decimal price)
        {
            Symbol = symbol;
            Price = price;
            Timestamp = DateTime.UtcNow;
        }
    }
}
