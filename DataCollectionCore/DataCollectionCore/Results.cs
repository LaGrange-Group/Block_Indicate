using System;
using System.Collections.Generic;

namespace DataCollectionCore
{
    public partial class Results
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal MaxPercentDiff24Hr { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Vwap { get; set; }
        public double PercentageChangeAf { get; set; }
        public decimal VolumeAf { get; set; }
        public decimal BitcoinVolumeAf { get; set; }
        public decimal BitcoinVolumeOriginal { get; set; }
        public int PrevRowId { get; set; }
        public bool? ResultOfTrade { get; set; }
        public bool Open { get; set; }
        public string TimeToResult { get; set; }
        public string Type { get; set; }
        public string Exchange { get; set; }
        public DateTime RealTime { get; set; }
        public bool? NoTrade { get; set; }
    }
}
