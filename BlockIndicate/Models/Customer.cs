using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Models
{
    public class Customer
    {
        [Key]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BinanceApiKey { get; set; }
        public string BinanceApiSecret { get; set; }
        public string BittrexApiKey { get; set; }
        public string BittrexApiSecret { get; set; }
        public bool AccountActive { get; set; }
        public bool AccountTrial { get; set; }
        public DateTime TrialStartDate { get; set; }
        public DateTime PaidStartDate { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
