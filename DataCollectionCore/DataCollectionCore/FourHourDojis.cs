using System;
using System.Collections.Generic;

namespace DataCollectionCore
{
    public partial class FourHourDojis
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
        public decimal BitcoinVolumeToMatch { get; set; }
        public decimal PriceToMatch { get; set; }
        public bool Logged { get; set; }
    }
}
