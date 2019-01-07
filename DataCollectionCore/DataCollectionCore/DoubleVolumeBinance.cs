using System;
using System.Collections.Generic;

namespace DataCollectionCore
{
    public partial class DoubleVolumeBinance
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal LastPrice { get; set; }
        public decimal BitcoinVolume { get; set; }
        public double PercentageChange { get; set; }
        public DateTime RealTime { get; set; }
    }
}
