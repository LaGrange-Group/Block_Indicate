using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class BalanceDetails
    {
        public string Name { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal BitcoinValue { get; set; }
        public decimal USDValue { get; set; }

    }
}
