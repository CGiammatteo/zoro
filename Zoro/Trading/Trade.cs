using System.Collections.Generic;

namespace Zoro.Trading
{
    internal class Trade
    {
        public long TraderId { get; set; }
        public long FinalTradeId { get; set; }
        public double PercentIncrease { get; set; }

        public List<LimitedData.Item> Offer = new List<LimitedData.Item>();
        public List<LimitedData.Item> Recieve = new List<LimitedData.Item>();
    }
}
