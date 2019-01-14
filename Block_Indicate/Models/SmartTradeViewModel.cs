﻿using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Controllers
{
    public class SmartTradeViewModel
    {
        public Customer Customer { get; set; }
        public decimal TotalBTC { get; set; }
        public decimal EstimatedBTC { get; set; }
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
    }
}
