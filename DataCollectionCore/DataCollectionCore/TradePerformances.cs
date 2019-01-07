using System;
using System.Collections.Generic;

namespace DataCollectionCore
{
    public partial class TradePerformances
    {
        public int Id { get; set; }
        public double Dvbsuccess { get; set; }
        public double Dvbfail { get; set; }
        public string DvbavgTime { get; set; }
        public string FourDbavgTime { get; set; }
        public double FourDbsuccess { get; set; }
        public double FourDbfail { get; set; }
        public double DvbnoTrade { get; set; }
        public double DvbnoTradeAvgSettle { get; set; }
        public double FourDbnoTrade { get; set; }
        public double FourDbnoTradeAvgSettle { get; set; }
    }
}
