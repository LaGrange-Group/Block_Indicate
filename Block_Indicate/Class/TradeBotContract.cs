using Binance.Net;
using Binance.Net.Objects;
using Block_Indicate.Data;
using Block_Indicate.Models;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class TradeBotContract
    {
        private readonly ApplicationDbContext db;
        private string binanceApikey;
        private string binanceApiSecret;
        public TradeBotContract(ApplicationDbContext context, TradeBot tradeBot, int numOfActiveTrades, string userId, Result market)
        {
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            db = context;
            binanceApikey = customer.BinanceApiKey;
            binanceApiSecret = customer.BinanceApiSecret;
            Run(tradeBot, numOfActiveTrades, market);

        }
        public void Run(TradeBot tradeBot, int numOfActiveTrades, Result market)
        {
            decimal freeBitcoin;
            if (tradeBot.NumberOfTrades == 1)
            {
                freeBitcoin = tradeBot.AllocatedBitcoin;
            }else if (tradeBot.NumberOfTrades == 2)
            {
                freeBitcoin = tradeBot.AllocatedBitcoin / 2m;
            }
            else if (tradeBot.NumberOfTrades == 3)
            {
                freeBitcoin = tradeBot.AllocatedBitcoin / 3m;
            }
            else
            {
                freeBitcoin = tradeBot.AllocatedBitcoin / 4m;
            }
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(binanceApikey, binanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                decimal askPrice = client.GetBookPrice(market.Symbol).Data.AskPrice;
                decimal amount = freeBitcoin / askPrice;
                var marketBuy = client.PlaceOrder(market.Symbol, OrderSide.Buy, OrderType.Market, amount);
                if (marketBuy.Success)
                {
                    decimal sellPrice = (market.LastPrice * 0.02m) + market.LastPrice;
                    string asset = market.Symbol.Remove(market.Symbol.Length - 2, market.Symbol.Length);
                    var assetAmount = client.GetAccountInfo().Data.Balances.Where(a => a.Asset == asset).Select(a => a.Total).Single();
                    var sellLimitOrder = client.PlaceOrder(market.Symbol, OrderSide.Sell, OrderType.Limit, assetAmount, null, sellPrice);
                    if (sellLimitOrder.Success)
                    {
                        ScanForConclusion();
                    }
                }
            }
        }

        private void ScanForConclusion()
        {
         
        }
    }
}
