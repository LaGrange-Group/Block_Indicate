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
using Telegram.Bot;

namespace Block_Indicate.Class
{
    public class TradeBotContract
    {
        private readonly ApplicationDbContext db;
        private string binanceApikey;
        private string binanceApiSecret;
        public TradeBotContract(ApplicationDbContext context, TradeBot tradeBot, int numOfActiveTrades, string userId, Result market)
        {
            
            db = context;

            Run(tradeBot, numOfActiveTrades, market, userId);

        }
        public void Run(TradeBot tradeBot, int numOfActiveTrades, Result market, string userId)
        {
            Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
            binanceApikey = customer.BinanceApiKey;
            binanceApiSecret = customer.BinanceApiSecret;
            var botClient = new TelegramBotClient("742774159:AAEWX5b7ZYkorD5-L64apFG6PdIjs57MTYY");
            botClient.SendTextMessageAsync(
              chatId: 542294321,
              text: "Creating New Trade for " + market.Symbol
            );
            decimal freeBitcoin;
            if (tradeBot.NumberOfTrades == 1)
            {
                freeBitcoin = tradeBot.AllocatedBitcoin - (tradeBot.AllocatedBitcoin * 0.10m);
            }else if (tradeBot.NumberOfTrades == 2)
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
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(binanceApikey, binanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                decimal askPrice = client.GetBookPrice(market.Symbol).Data.AskPrice;
                int decimalCount = BitConverter.GetBytes(decimal.GetBits(askPrice)[3])[2];
                decimal amount = Decimal.Round(freeBitcoin / askPrice, 0);
                var marketBuy = client.PlaceOrder(market.Symbol, OrderSide.Buy, OrderType.Market, amount);
                if (marketBuy.Success)
                {
                    botClient.SendTextMessageAsync(
                      chatId: 542294321,
                      text: "Successfuly Bought" + marketBuy.Data.Symbol + "At Price " + marketBuy.Data.Fills[0].Price.ToString()
                    );
                    decimal sellPrice = Decimal.Round(Convert.ToDecimal(((market.LastPrice * 0.02m) + market.LastPrice).ToString("0.#######################")), decimalCount);
                    string asset = market.Symbol.Remove(market.Symbol.Length - 3, 3);
                    decimal assetAmount = client.GetAccountInfo().Data.Balances.Where(a => a.Asset == asset).Select(a => a.Total).Single();
                    var sellLimitOrder = client.PlaceTestOrder(market.Symbol, OrderSide.Sell, OrderType.Limit, assetAmount + 1000, null, sellPrice, TimeInForce.GoodTillCancel);
                    
                    if (sellLimitOrder.Success)
                    {
                        //long? orderId = sellLimitOrder.Data.OrderId;
                        botClient.SendTextMessageAsync(
                          chatId: 542294321,
                          text: "Successfuly Set Take Profit At: " + assetAmount.ToString() + "For " + market.Symbol + " At Price: " + sellPrice.ToString()
                        );
                        ScanForConclusion(tradeBot, market, assetAmount, decimalCount/*, orderId*/);
                    }
                }
            }
        }

        private void ScanForConclusion(TradeBot tradeBot, Result market, decimal assetAmount, int decimalCount, long? orderId = null)
        {
            decimal stopLossPrice = Decimal.Round(market.LastPrice - (market.LastPrice * 0.10m), decimalCount);
            bool active = true;
            var botClient = new TelegramBotClient("742774159:AAEWX5b7ZYkorD5-L64apFG6PdIjs57MTYY");
            botClient.SendTextMessageAsync(
                chatId: 542294321,
                text: "Scanning for Stop Loss at: " + stopLossPrice.ToString()
            );
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(market.Symbol, KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                    if (data.Data.Low < stopLossPrice)
                    {
                        SellAtMarket(tradeBot, market, assetAmount, orderId);
                        active = false;
                    }

                });
                while (active)
                {

                }
            }
        }
        private void SellAtMarket(TradeBot tradeBot, Result market, decimal assetAmount, long? orderId)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(binanceApikey, binanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var cancelOrders = client.CancelOrder(market.Symbol, orderId);
                if (cancelOrders.Success)
                {
                    var sellMarket = client.PlaceOrder(market.Symbol, OrderSide.Sell, OrderType.Market, assetAmount);
                    if (sellMarket.Success)
                    {
                        return;
                    }
                }
            }
        }
    }
}
