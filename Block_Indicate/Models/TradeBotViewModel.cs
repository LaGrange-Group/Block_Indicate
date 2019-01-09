using Block_Indicate.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class TradeBotViewModel
    {
        public Customer Customer { get; set; }
        public decimal TotalBTC { get; set; }
        public decimal EstimatedBTC { get; set; }
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
        public List<TradeBot> TradeBots { get; set; }
        public TradeBot TradeBot { get; set; } 
    }
}
