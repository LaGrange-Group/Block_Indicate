using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class HuobiData
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal High { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Low { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Open { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Close { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Amount{ get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal DayAmount{ get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal KlineVolume{ get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal DayVolume{ get; set; }
        public int TradeCount{ get; set; }
        public int DayTradeCount{ get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime RealTime { get; set; }
    }
}
