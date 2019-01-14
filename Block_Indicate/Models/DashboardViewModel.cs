using Block_Indicate.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class DashboardViewModel
    {
        public Customer Customer { get; set; }
        public int ActiveBots { get; set; }
        public int BotsTotal { get; set; }
        public List<BalanceDetails> BalanceDetails { get; set; }
        public List<Tuple<string, decimal>> HuobiBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
        public List<ValidDoubleVolumeBinance> ValidDoubleVolumesBinance { get; set; }
        public List<TriggeredDojiFourHour> ValidDojiFourHoursBinance { get; set; }
        public TradePerformance TradePerformance { get; set; }
        public bool BinanceConnected { get; set; }
        public bool HuobiConnected { get; set; }
        public decimal TotalBTC { get; set; }
        public decimal ETHPrice { get; set; }
    }
}
