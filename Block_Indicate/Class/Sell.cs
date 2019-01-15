using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using Block_Indicate.Data;
using Block_Indicate.Models;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Telegram.Bot;

namespace Block_Indicate.Class
{
    public class Sell
    {
        public bool Market(Trade trade, Customer customer, int attempt = 0)
        {
            if (attempt < 5)
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(customer.BinanceApiKey, customer.BinanceApiSecret),
                    LogVerbosity = LogVerbosity.Debug,
                    LogWriters = new List<TextWriter> { Console.Out }
                });
                using (var client = new BinanceClient())
                {
                    var cancelOrder = client.CancelOrder(trade.Symbol, trade.OrderId);
                    if (cancelOrder.Success)
                    {
                        var marketSell = client.PlaceOrder(trade.Symbol, OrderSide.Sell, OrderType.Market, trade.Amount);
                        if (marketSell.Success)
                        {
                            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                            botClient.SendTextMessageAsync(
                                chatId: 542294321,
                                text: "Has market sold " + trade.Amount.ToString("0.###############") + " " + trade.Symbol + " at price " + marketSell.Data.Fills[0].Price.ToString()
                                );
                            return true;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(3000);
                            attempt = attempt + 1;
                            return Market(trade, customer, attempt);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000);
                        attempt = attempt + 1;
                        return Market(trade, customer, attempt);
                    }
                }
            }
            else
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Has attempted market sell 5 times for " + trade.Amount.ToString() + " " + trade.Symbol
                    );
                return false;
            }
        }
        public decimal MarketReturn(Trade trade, Customer customer, int attempt = 0)
        {
            if (attempt < 5)
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(customer.BinanceApiKey, customer.BinanceApiSecret),
                    LogVerbosity = LogVerbosity.Debug,
                    LogWriters = new List<TextWriter> { Console.Out }
                });
                using (var client = new BinanceClient())
                {
                    var cancelOrder = client.CancelOrder(trade.Symbol, trade.OrderId);
                    if (cancelOrder.Success)
                    {
                        var marketSell = client.PlaceOrder(trade.Symbol, OrderSide.Sell, OrderType.Market, trade.Amount);
                        if (marketSell.Success)
                        {
                            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                            botClient.SendTextMessageAsync(
                                chatId: 542294321,
                                text: "Has market sold " + trade.Amount.ToString("0.###############") + " " + trade.Symbol + " at price " + marketSell.Data.Fills[0].Price.ToString()
                                );
                            return marketSell.Data.Fills[0].Price;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(3000);
                            attempt = attempt + 1;
                            return MarketReturn(trade, customer, attempt);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000);
                        attempt = attempt + 1;
                        return MarketReturn(trade, customer, attempt);
                    }
                }
            }
            else
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Has attempted market sell 5 times for " + trade.Amount.ToString() + " " + trade.Symbol
                    );
                return 0;
            }
        }
        public async Task<bool> MarketAsync(Trade trade, Customer customer, int attempt = 0)
        {
            if (attempt < 5)
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(customer.BinanceApiKey, customer.BinanceApiSecret),
                    LogVerbosity = LogVerbosity.Debug,
                    LogWriters = new List<TextWriter> { Console.Out }
                });
                using (var client = new BinanceClient())
                {
                    var cancelOrder = await client.CancelOrderAsync(trade.Symbol, trade.OrderId);
                    if (cancelOrder.Success)
                    {
                        var marketSell = await client.PlaceOrderAsync(trade.Symbol, OrderSide.Sell, OrderType.Market, trade.Amount);
                        if (marketSell.Success)
                        {
                            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                            await botClient.SendTextMessageAsync(
                                chatId: 542294321,
                                text: "Has market sold " + trade.Amount.ToString("0.###############") + " " + trade.Symbol + " at price " + marketSell.Data.Fills[0].Price.ToString()
                                );
                            return true;
                        }
                        else
                        {
                            await Task.Delay(3000);
                            return await MarketAsync(trade, customer, attempt++);
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        return await MarketAsync(trade, customer, attempt++);
                    }
                }
            }
            else
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                await botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Has attempted market sell 5 times for " + trade.Amount.ToString() + " " + trade.Symbol
                    );
                return false;
            }
        }

        public async Task<BuildTrade> LimitAsync(BuildTrade build)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(build.Customer.BinanceApiKey, build.Customer.BinanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var sellLimit = await client.PlaceOrderAsync(build.Trade.Symbol, OrderSide.Sell, OrderType.Limit, build.Trade.Amount, null, build.Trade.TakeProfit, TimeInForce.GoodTillCancel);
                if (sellLimit.Success)
                {
                    build.Trade.OrderId = sellLimit.Data.OrderId;
                    
                    return build;
                }
            }
            // bool
            return build;
        }

        public BuildTrade MarketNoOrder(BuildTrade build)
        {
            using (var client = new BinanceClient())
            {
                var sellMarket = client.PlaceOrder(build.Trade.Symbol, OrderSide.Sell, OrderType.Market, build.Trade.Amount);
                if (sellMarket.Success)
                {
                    build.Trade.SellPrice = sellMarket.Data.Fills[0].Price;
                    build.Trade.Active = false;
                    return build;
                }
            }
            return build;
        }
    }
}
