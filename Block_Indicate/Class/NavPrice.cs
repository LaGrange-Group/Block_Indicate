using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
namespace Block_Indicate.Class
{
    public static class NavPrice
    {
        public static Dictionary<string, double> currentPrices = new Dictionary<string, double>();
        public static void CurrentPrices(DateTime nextTime)
        {
            try
            {
                using (var client = new BinanceClient())
                {
                    double btc = Convert.ToDouble(client.GetPrice("BTCUSDT").Data.Price);
                    double eth = Convert.ToDouble(client.GetPrice("ETHUSDT").Data.Price);
                    currentPrices.Add("btc", btc);
                    currentPrices.Add("eth", eth);
                }
            }
            catch
            {
                CurrentPrices(nextTime);
            }
            CountUntilNextMin(nextTime);
        }
        public static void CountUntilNextMin(DateTime nextTime)
        {
            do
            {
                System.Threading.Thread.Sleep(5000);
            } while (DateTime.Now < nextTime);
            nextTime = nextTime.AddMinutes(5);
            CurrentPrices(nextTime);
        }
    }
}
