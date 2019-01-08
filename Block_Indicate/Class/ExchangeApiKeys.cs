using Binance.Net;
using Binance.Net.Objects;
using Block_Indicate.Data;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Huobi.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public static class ExchangeApiKeys
    {

        public static bool? BinanceConnection;
        public static bool? HuobiConnection;
        public static string BinanceApiKey;
        public static string BinanceApiSecret;
        public static string HuobiApiKey;
        public static string HuobiApiSecret;
        public static List<string> activeExchanges = new List<string>();

        public static void Set(ApplicationDbContext context, string userId)
        {
            ApplicationDbContext db;
            db = context;
            BinanceConnection = db.Customers.Where(c => c.UserId == userId).Select(c => c.ConnectedBinance).Single();
            HuobiConnection = db.Customers.Where(c => c.UserId == userId).Select(c => c.ConnectedHuobi).Single();
            if (BinanceConnection == true)
            {
                BinanceApiKey = db.Customers.Where(c => c.UserId == userId).Select(c => c.BinanceApiKey).Single();
                BinanceApiSecret = db.Customers.Where(c => c.UserId == userId).Select(c => c.BinanceApiSecret).Single();
                activeExchanges.Add("Binance");
            }
            if (HuobiConnection == true)
            {
                HuobiApiKey = db.Customers.Where(c => c.UserId == userId).Select(c => c.HuobiApiKey).Single();
                HuobiApiSecret = db.Customers.Where(c => c.UserId == userId).Select(c => c.HuobiApiSecret).Single();
                activeExchanges.Add("Huobi");
            }
        }

        public static bool AttemptConnection(string apiKey, string apiSecrect, string exchange)
        {
            if (exchange == "Binance")
            {
                BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                {
                    ApiCredentials = new ApiCredentials(apiKey, apiSecrect),
                    LogVerbosity = LogVerbosity.Debug,
                    LogWriters = new List<TextWriter> { Console.Out }
                });
                using (var client = new BinanceClient())
                {
                    var accountInfo = client.GetAccountInfo();
                    if (accountInfo.Success)
                    {
                        AddExchangeIfNotExistent(exchange);
                        return true;
                    }
                }
            }else if(exchange == "Huobi")
            {
                using ( var client = new HuobiClient())
                {
                    client.SetApiCredentials(apiKey, apiSecrect);
                    var accounts = client.GetAccounts();
                    if (accounts.Success)
                    {
                        AddExchangeIfNotExistent(exchange);
                        return true;
                        
                    }
                }
            }
            return false;
        }

        private static void AddExchangeIfNotExistent(string newExchange)
        {
            bool exists = false;
            foreach (string exchange in activeExchanges)
            {
                if (exchange == newExchange)
                {
                    exists = true;
                }
            }
            if (exists == false)
            {
                activeExchanges.Add(newExchange);
            }
        }

        public static List<Tuple<string, decimal>> GetAccountBalances(string exchange)
        {
            try
            {
                if (exchange == "Binance")
                {
                    BinanceClient.SetDefaultOptions(new BinanceClientOptions()
                    {
                        ApiCredentials = new ApiCredentials(BinanceApiKey, BinanceApiSecret),
                        LogVerbosity = LogVerbosity.Debug,
                        LogWriters = new List<TextWriter> { Console.Out }
                    });
                    using (var client = new BinanceClient())
                    {
                        var accountInfo = client.GetAccountInfo();
                        if (accountInfo.Success)
                        {
                            var balances = accountInfo.Data.Balances;
                            List<Tuple<string, decimal>> valid = new List<Tuple<string, decimal>>();
                            valid = valid.OrderBy(v => v.Item2).ToList();

                            foreach (var market in balances)
                            {
                                if (market.Total > 0 || market.Asset == "BTC")
                                {
                                    Tuple<string, decimal> tuple = new Tuple<string, decimal>(market.Asset, market.Total);
                                    valid.Add(tuple);
                                }
                            }
                            valid.Add(GetEstimatedBTC(valid));
                            return valid;
                        }
                    }
                }
                else if (exchange == "Huobi")
                {
                    using (var client = new HuobiClient())
                    {
                        client.SetApiCredentials(HuobiApiKey, HuobiApiSecret);
                        var accounts = client.GetAccounts();
                        var balances = client.GetBalances(accounts.Data[0].Id).Data;
                        if (accounts.Success)
                        {
                            List<Tuple<string, decimal>> valid = new List<Tuple<string, decimal>>();
                            foreach (var market in balances)
                            {
                                if (market.Balance > 0)
                                {
                                    Tuple<string, decimal> tuple = new Tuple<string, decimal>(market.Currency, market.Balance);
                                    valid.Add(tuple);
                                }
                            }
                            return valid;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return GetAccountBalances(exchange);
            }
        }

        private static Tuple<string, decimal> GetEstimatedBTC(List<Tuple<string, decimal>> balances)
        {
            decimal totalBTC = 0m;
            foreach (Tuple<string, decimal> balance in balances)
            {
                decimal btcAmount = RetrieveBTCAmount(balance.Item1, balance.Item2);
                totalBTC += btcAmount;
            }
            Tuple<string, decimal> tuple = new Tuple<string, decimal>("EstimatedBTC", totalBTC);
            return tuple;
        }

        private static decimal RetrieveBTCAmount(string symbol, decimal amount)
        {
            try
            {
                using (var client = new BinanceClient())
                {
                    if (symbol == "BTC")
                    {
                        var token = client.Get24HPrice(symbol + "USDT").Data;
                        decimal btcAmount = amount;
                        return btcAmount;
                    }
                    else
                    {
                        var token = client.Get24HPrice(symbol + "BTC").Data;
                        decimal btcAmount = token.LastPrice * amount;
                        return btcAmount;
                    }

                }
            }
            catch
            {
                return 0m;
            }
        }
    }
}
