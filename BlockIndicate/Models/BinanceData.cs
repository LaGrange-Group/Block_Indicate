using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class BinanceData
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Close { get; set; }
        public DateTime? CloseTime { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal High { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Low { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Open { get; set; }
        public DateTime? OpenTime { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal QuoteAssetVolume { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal TakerBuyBaseAssetVolume { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal TakerBuyQuoteAssetVolume { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal TradeCount { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal Volume { get; set; }
        [Column(TypeName = "decimal(18, 10)")]
        public decimal BitcoinVolume { get; set; }
        [Column(TypeName = "decimal(18, 5)")]
        public decimal PercentageChange { get; set; }

        public DateTime RealTime { get; set; }
    }
}
