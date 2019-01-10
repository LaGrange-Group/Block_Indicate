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
        public TradeBotContract(ApplicationDbContext context, TradeBot tradeBot, int numOfActiveTrades, int customerId, Result market)
        {
            
            db = context;
            Run(tradeBot, numOfActiveTrades, market, customerId);

        }
        public void Run(TradeBot tradeBot, int numOfActiveTrades, Result market, int customerId)
        {
            try
            {
                Trade trade = new Trade();
                trade.Symbol = market.Symbol;
                trade.Exchange = tradeBot.Exchange;
                trade.CustomerId = customerId;
                Customer customer = db.Customers.Where(c => c.Id == customerId).Single();
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
                    trade.StartingBitcoinAmount = freeBitcoin;
                    trade.Amount = amount;
                    var marketBuy = client.PlaceOrder(market.Symbol, OrderSide.Buy, OrderType.Market, amount);
                    if (marketBuy.Success)
                    {
                        botClient.SendTextMessageAsync(
                          chatId: 542294321,
                          text: "Successfuly Bought " + marketBuy.Data.Symbol + " At Price " + marketBuy.Data.Fills[0].Price.ToString()
                        );
                        trade.BuyPrice = marketBuy.Data.Fills[0].Price;
                        trade.StartDate = DateTime.Now;
                        trade.Active = true;
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
                              text: "Successfuly set take profit for " + amount.ToString() + " " + market.Symbol + " at price " + sellPrice.ToString()
                            );
                            trade.TakeProfit = sellPrice;
                            ScanForConclusion(tradeBot, market, amount, decimalCount, orderId, trade);
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

        private void ScanForConclusion(TradeBot tradeBot, Result market, decimal assetAmount, int decimalCount, long? orderId, Trade trade)
        {
            try
            {
                decimal stopLossPrice = Decimal.Round(market.LastPrice - (market.LastPrice * 0.10m), decimalCount);
                trade.StopLoss = stopLossPrice;
                AddTrade(trade);
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
                            SellAtMarket(tradeBot, market, assetAmount, orderId, trade);
                            active = false;
                            return;
                        }

                    });

                    while (active)
                    {
                        using (var clientRest = new BinanceClient())
                        {
                            var orderStatus = clientRest.QueryOrder(market.Symbol, orderId);

                            if (orderStatus.Data.ExecutedQuantity == assetAmount)
                            {
                                decimal percentGain = (orderStatus.Data.Price - trade.BuyPrice) / trade.BuyPrice * 100;
                                botClient.SendTextMessageAsync(
                                    chatId: 542294321,
                                    text: "--Trade Concluded-- \nCoin: " + market.Symbol + "\nGain: + " + percentGain + " %"
                                );
                                trade.SellPrice = orderStatus.Data.Price;
                                UpdateTrade(trade);
                                active = false;
                                return;
                            }
                        }
                        System.Threading.Thread.Sleep(10000);
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
        private void SellAtMarket(TradeBot tradeBot, Result market, decimal assetAmount, long? orderId, Trade trade)
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
                            trade.SellPrice = sellMarket.Data.Fills[0].Price;
                            UpdateTrade(trade);
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
        private void UpdateTrade(Trade trade)
        {
            using (var db = new ApplicationDbContext())
            {
                trade.EndingBitcoinAmount = trade.SellPrice * trade.Amount;
                trade.FinalPercentageResult = Decimal.Round((trade.SellPrice - trade.BuyPrice) / trade.BuyPrice * 100, 2);
                trade.Active = false;
                trade.Closed = true;
                trade.EndDate = DateTime.Now;
                db.Update(trade);
                db.SaveChanges();
            }
        }
        private void AddTrade(Trade trade)
        {
            using (var db = new ApplicationDbContext())
            {
                trade.RealTime = DateTime.Now;
                db.Trades.Add(trade);
                db.SaveChanges();
            }
        }
    }
}
