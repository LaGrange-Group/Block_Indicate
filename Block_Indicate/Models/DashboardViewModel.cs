using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class DashboardViewModel
    {
        public Dictionary<string, decimal> BinanceBalances { get; set; }
        public Dictionary<string, decimal> HuobiBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
        public bool BinanceConnected { get; set; }
        public bool HuobiConnected { get; set; }
    }
}
