using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class GetBalanceDetails
    {
        public async Task<List<BalanceDetails>> GetDetails()
        {
            List<BalanceDetails> balanceDetails = new List<BalanceDetails>();
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(ExchangeApiKeys.BinanceApiKey, ExchangeApiKeys.BinanceApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var accountInfo = await client.GetAccountInfoAsync();
                if (accountInfo.Success)
                {
                    var balances = accountInfo.Data.Balances;
                    var btc = await client.Get24HPriceAsync("BTCUSDT");
                    if (btc.Success)
                    {
                        BalanceDetails bitcoin = new BalanceDetails();
                        bitcoin.Name = "BTCUSDT";
                        bitcoin.LastPrice = btc.Data.LastPrice;
                        balanceDetails.Add(bitcoin);
                        balanceDetails.Add(await GetSpecificSymbol("ETHUSDT"));
                        foreach (var market in balances)
                        {
                            BalanceDetails details = new BalanceDetails();
                            if (market.Total > 0 || market.Asset == "BTC")
                            {
                                details.Name = market.Asset;
                                details.Amount = market.Total;
                                string symbol = market.Asset != "BTC" ? market.Asset + "BTC" : market.Asset + "USDT";
                                var currentDetails = await client.Get24HPriceAsync(symbol);
                                if (currentDetails.Success)
                                {
                                    details.LastPrice = currentDetails.Data.LastPrice;
                                    details.BitcoinValue = details.Name != "BTC" ? details.Amount * currentDetails.Data.LastPrice : details.Amount;
                                    details.USDValue = Decimal.Round(btc.Data.LastPrice * details.BitcoinValue, 2);
                                }
                                balanceDetails.Add(details);
                            }
                        }
                    }
                    balanceDetails.Add(GetEstimatedBTC(balanceDetails, btc.Data.LastPrice));
                    return balanceDetails;
                }
            }
            return null;
        }
        private BalanceDetails GetEstimatedBTC(List<BalanceDetails> balances, decimal bitcoinPrice)
        {
            decimal btcTotal = 0;
            BalanceDetails details = new BalanceDetails();
            foreach (BalanceDetails detail in balances)
            {
                btcTotal = detail.Name != "BTC" ? btcTotal + detail.BitcoinValue : detail.Amount;
            }
            details.Name = "BitcoinTotal";
            details.BitcoinValue = btcTotal;
            details.Amount = btcTotal;
            details.LastPrice = bitcoinPrice;
            details.USDValue = btcTotal * bitcoinPrice;
            return (details);
        }

        private async Task<BalanceDetails> GetSpecificSymbol(string symbol)
        {
            BalanceDetails details = new BalanceDetails();
            using (var client = new BinanceClient())
            {
                var currentPrice = await client.Get24HPriceAsync(symbol);
                if (currentPrice.Success)
                {
                    details.Name = symbol;
                    details.LastPrice = currentPrice.Data.LastPrice;
                }
            }
            return details;
        }
    }
}
