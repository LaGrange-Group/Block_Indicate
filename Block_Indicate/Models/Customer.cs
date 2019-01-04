using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string BinanceApiKey { get; set; }
        public string BinanceApiSecret { get; set; }
        public string HuobiApiKey { get; set; }
        public string HuobiApiSecret { get; set; }
        public bool AccountActive { get; set; }
        public bool AccountTrial { get; set; }
        public bool CompletedSignUp { get; set; }
        public bool? ConnectedBinance { get; set; }
        public bool? ConnectedHuobi { get; set; }
        public DateTime TrialStartDate { get; set; }
        public DateTime PaidStartDate { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
