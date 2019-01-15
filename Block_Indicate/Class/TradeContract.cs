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
    public class TradeContract
    {
        private readonly ApplicationDbContext db;
        private string binanceApikey;
        private string binanceApiSecret;
        public TradeContract(ApplicationDbContext context, TradeBot tradeBot, int numOfActiveTrades, int customerId, Result market)
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
                trade.Type = "Bot";
                BuildName buildName = new BuildName();
                trade.Name = buildName.BuildNameSpaces(tradeBot.Name);
                Customer customer = db.Customers.Where(c => c.Id == customerId).Single();
                binanceApikey = customer.BinanceApiKey;
                binanceApiSecret = customer.BinanceApiSecret;
                CalculateBaseCurrency calculateBaseCurrency = new CalculateBaseCurrency();
                decimal freeBaseCurrency = calculateBaseCurrency.GetFreeBitcoin(tradeBot);
                trade.StartingBitcoinAmount = freeBaseCurrency;
                Notify notify = new Notify();
                notify.Telegram(customerId, "Creating New Trade for " + market.Symbol);

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
                    CalculateAmount calculateAmount = new CalculateAmount();
                    trade.Amount = calculateAmount.GetAmount(market.Symbol, askPrice, freeBaseCurrency);

                    // Place Market Order
                    var marketBuy = client.PlaceOrder(market.Symbol, OrderSide.Buy, OrderType.Market, trade.Amount);
                    if (marketBuy.Success)
                    {
                        notify.Telegram(customerId, "Successfuly Bought " + marketBuy.Data.Symbol + " At Price " + marketBuy.Data.Fills[0].Price.ToString());

                        // Set Trade Row Buy Price
                        trade.BuyPrice = marketBuy.Data.Fills[0].Price;
                        trade.StartDate = DateTime.Now;
                        trade.Active = true;
                        decimal sellPrice = Decimal.Round(Convert.ToDecimal(((trade.BuyPrice * (tradeBot.PercentTakeProfit / 100)) + trade.BuyPrice).ToString("0.#######################")), decimalCount);
                        string asset = market.Symbol.Remove(market.Symbol.Length - 3, 3);

                        // Place Sell Limit Order
                        var sellLimitOrder = client.PlaceOrder(market.Symbol, OrderSide.Sell, OrderType.Limit, trade.Amount, null, sellPrice, TimeInForce.GoodTillCancel);
                        if (sellLimitOrder.Success)
                        {
                            trade.OrderId = sellLimitOrder.Data.OrderId;
                            notify.Telegram(customerId, "Successfuly set take profit for " + trade.Amount.ToString() + " " + market.Symbol + " at price " + sellPrice.ToString());
                            trade.TakeProfit = sellPrice;
                            ScanForConclusion(tradeBot, market, decimalCount, trade, customer);
                        }
                        else
                        {
                            notify.Telegram(customerId, "Trade Set Take Profit Failure : " + marketBuy.Error);
                        }
                    }
                    else
                    {
                        notify.Telegram(customerId, "Trade Buy Market Failure : " + marketBuy.Error);
                    }
                }
            }
            catch (Exception e)
            {
                Notify notify = new Notify();
                notify.Telegram(customerId, "Trade Contract Failure : Scan For Buy + Set Take Profit\nException: " + e);
            }
        }

        private void ScanForConclusion(TradeBot tradeBot, Result market, int decimalCount, Trade trade, Customer customer)
        {
            try
            {
                decimal decimalTimes = tradeBot.PercentStopLoss / 100;
                decimal twoPerc = trade.BuyPrice * (tradeBot.PercentStopLoss / 100);
                decimal stopLossPrice = Decimal.Round(trade.BuyPrice - (trade.BuyPrice * (tradeBot.PercentStopLoss / 100) * -1), decimalCount);
                trade.StopLoss = stopLossPrice;
                AddTrade(trade);
                bool active = true;
                Notify notify = new Notify();
                notify.Telegram(customer.Id, "Scanning for Stop Loss at: " + stopLossPrice.ToString());
                using (var client = new BinanceSocketClient())
                {
                    var successKline = client.SubscribeToKlineStream(market.Symbol, KlineInterval.OneMinute, (data) =>
                    {
                        // handle data
                        if (data.Data.Low < stopLossPrice && active == true)
                        {
                            
                            Sell sell = new Sell();
                            trade.SellPrice = sell.MarketReturn(trade, customer);
                            UpdateTrade(trade);
                            active = false;
                            return;
                        }

                    });

                    while (active)
                    {
                        using (var clientRest = new BinanceClient())
                        {
                            var orderStatus = clientRest.QueryOrder(market.Symbol, trade.OrderId);
                            if (orderStatus.Success)
                            {
                                if (orderStatus.Data.Status == OrderStatus.Filled || orderStatus.Data.Status == OrderStatus.Canceled)
                                {
                                    decimal percentGain = (orderStatus.Data.Price - trade.BuyPrice) / trade.BuyPrice * 100;
                                    notify.Telegram(customer.Id, "--Trade Concluded-- \nCoin: " + market.Symbol + "\nGain: + " + decimal.Round(percentGain, 2) + " %");
                                    trade.SellPrice = orderStatus.Data.Price;
                                    UpdateTrade(trade);
                                    active = false;
                                    return;
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(10000);
                    }
                }
            }
            catch (Exception e)
            {
                Notify notify = new Notify();
                notify.Telegram(customer.Id, "Trade Contract Failure : Scan For Conclusion\nException: " + e);
            }
        }
        private void UpdateTrade(Trade trade)
        {
            using (var db = new ApplicationDbContext())
            {
                trade.EndingBitcoinAmount = trade.SellPrice * trade.Amount;
                trade.FinalPercentageResult = Decimal.Round((trade.SellPrice - trade.BuyPrice) / trade.BuyPrice * 100, 2);
                trade.Active = false;
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
