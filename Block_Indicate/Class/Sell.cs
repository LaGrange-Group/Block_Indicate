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
    public static class Sell
    {
        public static async Task<bool> Market(string symbol, decimal amount, Customer customer, int attempt = 0)
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
                    var orders = await client.GetAllOrdersAsync(symbol);
                    if (orders.Success)
                    {
                        var order = orders.Data.Where(o => o.Status == OrderStatus.New).Single();
                        long orderId = order.OrderId;
                        var cancelOrder = await client.CancelOrderAsync(symbol, orderId);
                        if (cancelOrder.Success)
                        {
                            var marketSell = await client.PlaceOrderAsync(symbol, Binance.Net.Objects.OrderSide.Sell, Binance.Net.Objects.OrderType.Market, amount);
                            if (marketSell.Success)
                            {
                                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                                await botClient.SendTextMessageAsync(
                                    chatId: 542294321,
                                    text: "Has market sold " + amount.ToString() + " " + symbol + "at price " + marketSell.Data.Fills[0].Price.ToString()
                                    );
                            }
                            else
                            {
                                await Task.Delay(3000);
                                return await Market(symbol, amount, customer, attempt++);
                            }
                        }
                        else
                        {
                            await Task.Delay(3000);
                            return await Market(symbol, amount, customer, attempt++);
                        }
                    }
                    else
                    {
                        await Task.Delay(3000);
                        return await Market(symbol, amount, customer, attempt++);
                    }
                }
            }
            else
            {
                var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                await botClient.SendTextMessageAsync(
                    chatId: 542294321,
                    text: "Has attempted market sell 5 times for " + amount.ToString() + " " + symbol
                    );
                return false;
            }
            return false;
        }
    }
}
