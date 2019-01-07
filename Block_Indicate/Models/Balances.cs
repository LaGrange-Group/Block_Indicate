using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class Balances
    {
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public List<Tuple<string, decimal>> HuobiBalances { get; set; }
        public decimal totalBTC { get; set; }
    }
}
