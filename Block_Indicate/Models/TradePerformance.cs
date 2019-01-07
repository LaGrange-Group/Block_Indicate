using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class TradePerformance
    {
        [Key]
        public int Id { get; set; }
        public double DVBSuccess { get; set; }
        public double DVBFail { get; set; }
        public double DVBNoTrade { get; set; }
        public double DVBNoTradeAvgSettle { get; set; }
        public string DVBAvgTime { get; set; }
        public string FourDBAvgTime { get; set; }
        public double FourDBSuccess { get; set; }
        public double FourDBFail { get; set; }
        public double FourDBNoTrade { get; set; }
        public double FourDBNoTradeAvgSettle { get; set; }
    }
}
