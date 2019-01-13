using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; }
        public decimal TotalBTC { get; set; }
        public decimal EstimatedBTC { get; set; }
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
    }
}
