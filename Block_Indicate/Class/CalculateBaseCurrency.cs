using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class CalculateBaseCurrency
    {
        public decimal GetFreeBitcoin(TradeBot tradeBot)
        {
            decimal freeBitcoin;
            if (tradeBot.NumberOfTrades == 1)
            {
                freeBitcoin = tradeBot.AllocatedBitcoin - (tradeBot.AllocatedBitcoin * 0.10m);
            }
            else if (tradeBot.NumberOfTrades == 2)
            {
                freeBitcoin = (tradeBot.AllocatedBitcoin / 2m) - ((tradeBot.AllocatedBitcoin / 2m) * 0.10m);
            }
            else if (tradeBot.NumberOfTrades == 3)
            {
                freeBitcoin = (tradeBot.AllocatedBitcoin / 3m) - ((tradeBot.AllocatedBitcoin / 3m) * 0.10m);
            }
            else
            {
                freeBitcoin = (tradeBot.AllocatedBitcoin / 4m) - ((tradeBot.AllocatedBitcoin / 4m) * 0.10m);
            }
            return freeBitcoin;
        }
    }
}
