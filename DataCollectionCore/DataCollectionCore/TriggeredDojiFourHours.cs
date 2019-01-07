using System;
using System.Collections.Generic;

namespace DataCollectionCore
{
    public partial class TriggeredDojiFourHours
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public decimal BitcoinVolume { get; set; }
        public DateTime RealTime { get; set; }
        public decimal LastPrice { get; set; }
        public double PercentageChange { get; set; }
        public decimal Vwap { get; set; }
        public bool Logged { get; set; }
    }
}
