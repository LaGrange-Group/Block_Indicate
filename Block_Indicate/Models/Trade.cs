using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class Trade
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal CurrentPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BuyPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal TakeProfit { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal StopLoss { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal DiffFromStopLoss { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal DiffFromTakeProfit { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal CurrentDiff { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal StartingBitcoinAmount { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal EndingBitcoinAmount { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal CurrentPercentageResult { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal FinalPercentageResult { get; set; }
        public string TimeActive { get; set; }
        public string TimeToCompletion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Active { get; set; }
        public bool Closed { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime RealTime { get; set; }

    }
}
