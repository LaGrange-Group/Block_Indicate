using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class TradeBot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int NumberOfTrades { get; set; }
        public int NumberOfActiveTrades { get; set; }
        public bool AllMarkets { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal AllocatedBitcoin { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal PercentTakeProfit {get; set;}
        [Column(TypeName = "decimal(28, 18)")]
        public decimal PercentStopLoss { get; set; }
        public string Exchange { get; set; }
        public string BaseCurrency { get; set; }
        public bool Status { get; set; }
        public int UniqueSetId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
