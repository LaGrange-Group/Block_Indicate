using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class AccountPerformance
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal USD { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BTC { get; set; }
        public DateTime DateUpdated { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
