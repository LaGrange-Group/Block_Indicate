using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
namespace Block_Indicate.Class
{
    public class NavPrice
    {
        public Dictionary<string, double> CurrentPrices()
        {
            Dictionary<string, double> currentPrices = new Dictionary<string, double>();
            try
            {
                using (var client = new BinanceClient())
                {
                    double btc = Convert.ToDouble(client.GetPrice("BTCUSDT").Data.Price);
                    double eth = Convert.ToDouble(client.GetPrice("ETHUSDT").Data.Price);
                    currentPrices.Add("btc", btc);
                    currentPrices.Add("eth", eth);
                }
                return currentPrices;
            }
            catch
            {
                System.Threading.Thread.Sleep(5000);
                return CurrentPrices();
            }
        }
        public async Task<Dictionary<string, double>> CurrentPricesAsync()
        {
            Dictionary<string, double> currentPrices = new Dictionary<string, double>();
            using (var client = new BinanceClient())
            {
                var btcA = await client.GetPriceAsync("BTCUSDT");
                var ethA = await client.GetPriceAsync("ETHUSDT");
                double btc = Convert.ToDouble(btcA.Data.Price);
                double eth = Convert.ToDouble(ethA.Data.Price);
                currentPrices.Add("btc", btc);
                currentPrices.Add("eth", eth);
            }
            return currentPrices;
        }

        //public static void CountUntilNextMin(DateTime nextTime)
        //{
        //    do
        //    {
        //        System.Threading.Thread.Sleep(5000);
        //    } while (DateTime.Now < nextTime);
        //    nextTime = nextTime.AddMinutes(5);
        //    CurrentPrices(nextTime);
        //}
    }
}
