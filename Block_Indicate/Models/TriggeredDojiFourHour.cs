using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class TriggeredDojiFourHour
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Open { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Close { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal High { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Low { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal LastPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Vwap { get; set; }
        public double PercentageChange { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Volume { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BitcoinVolume { get; set; }
        public bool Logged { get; set; }
        public DateTime RealTime { get; set; }
    }
}
