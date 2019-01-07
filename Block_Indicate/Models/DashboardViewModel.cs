using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class DashboardViewModel
    {
        public Customer Customer { get; set; }
        public decimal TotalBTC { get; set; }
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public List<Tuple<string, decimal>> HuobiBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
        public List<ValidDoubleVolumeBinance> ValidDoubleVolumesBinance { get; set; }
        public List<TriggeredDojiFourHour> ValidDojiFourHoursBinance { get; set; }
        public TradePerformance TradePerformance { get; set; }
        public bool BinanceConnected { get; set; }
        public bool HuobiConnected { get; set; }
        public decimal BitcoinPrice { get; set; }
        public decimal EthereumPrice { get; set; }
    }
}
