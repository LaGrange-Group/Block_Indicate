using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal LastPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Vwap { get; set; }
        public double PercentageChangeAF { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal VolumeAF { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BitcoinVolumeAF { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BitcoinVolumeOriginal {get; set;}
        [ForeignKey("DoubleVolumeBinance")]
        public int PrevRowId { get; set; }
        public DoubleVolumeBinance DoubleVolumeBinance { get; set; }
        public bool? ResultOfTrade { get; set; }
        public bool Open { get; set; }
        public bool? NoTrade { get; set; }
        public string TimeToResult { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal MaxPercentDiff24Hr { get; set; }
        public string Type { get; set; }
        public string Exchange { get; set; }
        public DateTime RealTime { get; set; }
    }
}
