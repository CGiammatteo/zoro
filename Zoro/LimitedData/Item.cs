using System;

namespace Zoro.LimitedData
{
    class Item
    {
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public double Score { get;  set; }
        public int RoundedRap { get; set; }
        public int Value { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsProjected { get; set; }
        public int AverageSales { get; set; }
    }
}
