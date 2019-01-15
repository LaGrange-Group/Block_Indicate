using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class Message
    {
        public async Task SmartCreated(BuildTrade build)
        {
            string msg = "--Smart Trade Created--";
            string lb = "\n";
            msg += lb + "Token: " + build.Trade.Symbol;
            msg += lb + "Amount: " + build.Trade.Amount.ToString();
            msg += lb + "Buy Price: " + build.Trade.BuyPrice.ToString();
            msg += lb + "Take Profit Price: " + build.Trade.TakeProfit.ToString();
            msg += lb + "Stop Loss Price: " + build.Trade.StopLoss.ToString();
            msg += lb + "Start Time: " + build.Trade.RealTime.ToString();
            Notify notify = new Notify();
            await notify.TelegramAsync(build.Customer.Id, msg);
        }

        public async Task SmartConcluded(BuildTrade build)
        {
            string msg = "--Smart Trade Concluded--";
            string lb = "\n";
            msg += lb + "Token: " + build.Trade.Symbol;
            msg += lb + "Amount: " + build.Trade.Amount.ToString();
            string percent = build.Trade.FinalPercentageResult < 0 ? "- " + build.Trade.FinalPercentageResult.ToString() : "+ " + build.Trade.FinalPercentageResult.ToString();
            msg += lb + "Result: " + percent + " %";
            Notify notify = new Notify();
            await notify.TelegramAsync(build.Customer.Id, msg);
        }
    }
}
