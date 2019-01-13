using Binance.Net;
using Block_Indicate.Models;
using System;
using Binance.Net.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class CalculateAmount
    {
        public decimal GetAmount(Result market, decimal askPrice, decimal freeBaseCurrency, int attempt = 0)
        {
            decimal amount = 0;
            if (attempt < 5)
            {
                using (var client = new BinanceClient())
                {
                    var orderBook = client.GetRecentTrades(market.Symbol, 20);
                    if (orderBook.Success)
                    {
                        List<BinanceRecentTrade> trades = orderBook.Data.ToList();
                        int decimalCount = GetLargestDecimalPlace(trades);
                        amount = Decimal.Round(freeBaseCurrency / askPrice, decimalCount);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(30000);
                        return GetAmount(market, askPrice, freeBaseCurrency, attempt++);
                    }
                }
                return amount;
            }
            else
            {
                return amount;
            }
        }

        private int GetLargestDecimalPlace(List<BinanceRecentTrade> trades)
        {
            int largestDecimalPlace = 0;
            foreach (BinanceRecentTrade trade in trades)
            {
                decimal quantity = Convert.ToDecimal(trade.Quantity.ToString("0.################"));
                int decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
                if (decimalCount > largestDecimalPlace)
                {
                    largestDecimalPlace = decimalCount;
                }
            }
            return largestDecimalPlace;
        }
    }
}
