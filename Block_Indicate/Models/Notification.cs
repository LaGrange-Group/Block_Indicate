using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int? TelegramChatId { get; set; }
        public bool TelegramDoubleVolume { get; set; }
        public bool TelegramFourHourDojis { get; set; }
        public bool TelegramSmartTradeConclusions { get; set; }
        public bool TelegramBotTradeConclusions { get; set; }
        public bool TelegramBotTradeBuys { get; set; }
        public bool TelegramSiteUpdates { get; set; }
        public bool EmailDoubleVolume { get; set; }
        public bool EmailFourHourDojis { get; set; }
        public bool EmailSmartTradeConclusions { get; set; }
        public bool EmailBotTradeConclusions { get; set; }
        public bool EmailBotTradeBuys { get; set; }
        public bool EmailSiteUpdates { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
