using Binance.Net;
using Binance.Net.Objects;
using Block_Indicate.Models;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class Buy
    {
        private BuildTrade build;
        public Buy(BuildTrade build)
        {
            this.build = build;
        }

        public async Task<BuildTrade> MarketAsync()
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(build.Customer.BinanceApiKey, build.Customer.BinanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var marketBuy = await client.PlaceOrderAsync(build.Trade.Symbol, OrderSide.Buy, OrderType.Market, build.Trade.Amount);
                if (marketBuy.Success)
                {
                    build.Trade.BuyPrice = marketBuy.Data.Fills[0].Price;
                    build.Trade.StartDate = DateTime.Now;
                    build.Trade.Active = true;
                    build.Trade.TakeProfit = Decimal.Round(Convert.ToDecimal(((build.Trade.BuyPrice * (build.Trade.TakeProfitPercent / 100)) + build.Trade.BuyPrice).ToString("0.#######################")), build.AskDecimalLength);
                    return build;
                }
            }
            return build;
        }
    }
}
