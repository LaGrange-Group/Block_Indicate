﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class ApiConnectionViewModel
    {
        public Customer Customer { get; set; }
        public decimal TotalBTC { get; set; }
        public List<Tuple<string, decimal>> BinanceBalances { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecrect { get; set; }
        public string Exchange { get; set; }
        public Dictionary<string, double> CurrentPrices { get; set; }
    }
}
