using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class BittrexData
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? High { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Low { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Volume { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Last { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? BaseVolume { get; set; }
        public DateTime TimeStamp { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Bid { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? Ask { get; set; }
        public int? OpenBuyOrders {get; set;}
        public int? OpenSellOrders { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? PrevDay { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal? PercentageChange { get; set; }
        public DateTime RealTime { get; set; }
    }
}
