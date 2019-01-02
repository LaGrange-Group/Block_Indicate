using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class DojiFinder
    {
        [Key]
        public int Id { get; set; }
        public double DojiUpperWick { get; set; }
        public double DojiLowerWick { get; set; }
        public double DojiUpperBody { get; set; }
        public double DojiLowerBody { get; set; }
        public double DojiVolumeIncrease { get; set; }
        public double DojiPercentageIncrease { get; set; }
        public bool DojiTextMessage { get; set; }
        public bool DojiEmail { get; set; }
        public bool AllMarkets { get; set; }
        public bool AllExchanges { get; set; }
    }
}
