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
            try
            {
                Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
                binanceApikey = customer.BinanceApiKey;
                binanceApiSecret = customer.BinanceApiSecret;
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                  chatId: 542294321,
                  text: "Creating New Trade for " + market.Symbol
                );
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
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(binanceApikey, binanceApiSecret),
                    LogVerbosity = LogVerbosity.Debug,
                    LogWriters = new List<TextWriter> { Console.Out }
                });
                using (var client = new BinanceClient())
                {
                    // Round Amount Correctly
                    decimal askPrice = Convert.ToDecimal(client.GetBookPrice(market.Symbol).Data.AskPrice.ToString("0.##############"));
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
                        decimal assetAmount = client.GetAccountInfo().Data.Balances.Where(a => a.Asset == asset).Select(a => a.Total).Single() + 500;
                        assetAmount = assetAmount > 1 ? Decimal.Round(assetAmount, 0) : Decimal.Round(assetAmount, 4);
                        var sellLimitOrder = client.PlaceOrder(market.Symbol, OrderSide.Sell, OrderType.Limit, amount, null, sellPrice, TimeInForce.GoodTillCancel);

                        if (sellLimitOrder.Success)
                        {
                            long? orderId = sellLimitOrder.Data.OrderId;
                            botClient.SendTextMessageAsync(
                              chatId: 542294321,
                              text: "Successfuly Set Take Profit At: " + amount.ToString() + "For " + market.Symbol + " At Price: " + sellPrice.ToString()
                            );
                            ScanForConclusion(tradeBot, market, amount, decimalCount, orderId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Trade Contract Failure : Scan For Buy + Set Take Profit\nException: " + e
                );
            }
        }

        private void ScanForConclusion(TradeBot tradeBot, Result market, decimal assetAmount, int decimalCount, long? orderId = null)
        {
            try
            {
                decimal stopLossPrice = Decimal.Round(market.LastPrice - (market.LastPrice * 0.10m), decimalCount);
                bool active = true;
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
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
                            return;
                        }

                    });

                    while (active)
                    {
                        using (var clientRest = new BinanceClient())
                        {
                            var orderStatus = clientRest.QueryOrder(market.Symbol, orderId);

                            if (orderStatus.Data.Status == OrderStatus.Filled)
                            {
                                decimal percentGain = (orderStatus.Data.Price - market.LastPrice) / market.LastPrice * 100;
                                botClient.SendTextMessageAsync(
                                    chatId: 542294321,
                                    text: "--Trade Concluded-- \nCoin: " + market.Symbol + "\nGain: + " + percentGain + " %"
                                );
                                active = false;
                                return;
                            }
                        }
                        System.Threading.Thread.Sleep(10);
                    }
                }
            }
            catch (Exception e)
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Trade Contract Failure : Scan For Conclusion\nException: " + e
                );
            }
        }
        private void SellAtMarket(TradeBot tradeBot, Result market, decimal assetAmount, long? orderId)
        {
            try
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
                            var orderStatus = client.QueryOrder(market.Symbol, sellMarket.Data.OrderId);
                            decimal percentGain = (orderStatus.Data.Price - market.LastPrice) / market.LastPrice * 100;
                            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                            botClient.SendTextMessageAsync(
                                chatId: 542294321,
                                text: "--Trade Concluded-- \nCoin: " + market.Symbol + "\nLoss: + " + percentGain + " %"
                            );
                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Trade Contract Failure : Sell Stop Loss\nException: " + e
                );
            }
        }
    }
}
