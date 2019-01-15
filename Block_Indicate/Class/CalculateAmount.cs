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
        public decimal GetAmount(string symbol, decimal askPrice, decimal freeBaseCurrency, int attempt = 0)
        {
            decimal amount = 0;
            if (attempt < 5)
            {
                using (var client = new BinanceClient())
                {

                    var orderBook = client.GetRecentTrades(symbol, 20);
                    if (orderBook.Success)
                    {
                        List<BinanceRecentTrade> trades = orderBook.Data.ToList();
                        int decimalCount = GetLargestDecimalPlace(trades);
                        amount = Decimal.Round(freeBaseCurrency / askPrice, decimalCount);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(30000);
                        return GetAmount(symbol, askPrice, freeBaseCurrency, attempt++);
                    }
                }
                return amount;
            }
            else
            {
                return amount;
            }
        }
        public async Task<BuildTrade> GetAmountAsync(BuildTrade build = null, int attempt = 0)
        {
            if (attempt < 5)
            {
                using (var client = new BinanceClient())
                {
                    var getAsk = await client.GetBookPriceAsync(build.Trade.Symbol);
                    if (getAsk.Success)
                    {
                        build.AskPrice = Convert.ToDecimal(getAsk.Data.AskPrice.ToString("0.##############"));
                        build.AskDecimalLength = BitConverter.GetBytes(decimal.GetBits(build.AskPrice)[3])[2];

                        var orderBook = await client.GetRecentTradesAsync(build.Trade.Symbol, 20);
                        if (orderBook.Success)
                        {
                            List<BinanceRecentTrade> trades = orderBook.Data.ToList();
                            int decimalCount = GetLargestDecimalPlace(trades);
                            build.Trade.Amount = Decimal.Round(build.Trade.AllocatedBitcoin / build.AskPrice, decimalCount);
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(30000);
                            return await GetAmountAsync(build, attempt++);
                        }
                    }
                }
                return build;
            }
            else
            {
                return build;
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
