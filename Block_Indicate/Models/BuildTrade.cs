using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class BuildTrade
    {
        public Customer Customer { get; set; }
        public Trade Trade { get; set; }
        public long? OrderId { get; set; }
        public decimal AskPrice { get; set; }
        public int AskDecimalLength { get; set; }
        public bool? TrailingTake { get; set; }
    }
}
