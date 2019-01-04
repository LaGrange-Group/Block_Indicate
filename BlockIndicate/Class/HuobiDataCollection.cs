using BlockIndicate.Data;
using BlockIndicate.Models;
using Huobi.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Class
{
    public class HuobiDataCollection
    {
        public ApplicationDbContext db;
        private int timeRan;
        private List<DateTime> timesFailed = new List<DateTime>();
        List<TimeSpan> times = new List<TimeSpan>();
        public HuobiDataCollection(ApplicationDbContext context)
        {
            db = context;
            timeRan = 0;
        }

        public void RunCollection(DateTime nextTime)
        {
            List<string> markets = new List<string>() { "xemusdt", "elausdt", "mdsusdt", "trxusdt", "thetausdt", "dtausdt", "letusdt", "htusdt", "nasusdt", "zilusdt", "ruffusdt", "itcusdt", "socusdt", "iotausdt", "qtumusdt", "etcusdt", "ltcusdt", "ethusdt", "wiccusdt", "cmtusdt", "hb10usdt", "ocnusdt", "xlmusdt", "gxcusdt", "steemusdt", "dashusdt", "omgusdt", "zecusdt", "eosusdt", "xrpusdt", "bchusdt", "btcusdt", "bixusdt", "paiusdt", "vetusdt", "hitusdt", "xmrusdt", "bsvusdt", "nanousdt", "wavesusdt", "linkusdt", "zrxusdt", "dcrusdt", "hptusdt" };
            DateTime currentTime = DateTime.Now;
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (string market in markets)
                {
                    HuobiData huobi = new HuobiData();
                    using (var client = new HuobiClient())
                    {
                        var klines = client.GetMarketKlines(market, Huobi.Net.Objects.HuobiPeriod.FiveMinutes, 2).Data[1];
                        var day = client.GetMarketTickerMerged(market).Data;
                        huobi.Symbol = market;
                        huobi.High = klines.High;
                        huobi.Low = klines.Low;
                        huobi.Open = klines.Open;
                        huobi.Close = klines.Close;
                        huobi.Amount = Math.Round(klines.Amount, 17);
                        huobi.DayAmount = Math.Round(day.Amount, 17);
                        huobi.KlineVolume = Math.Round(klines.Volume, 17);
                        huobi.DayVolume = Math.Round(day.Volume, 17);
                        huobi.TradeCount = klines.TradeCount;
                        huobi.DayTradeCount = day.TradeCount;
                        huobi.TimeStamp = day.Timestamp;
                        huobi.RealTime = currentTime;
                    }
                    db.HuobiData.Add(huobi);
                    db.SaveChanges();
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                times.Add(ts);
                timeRan++;
            }
            catch
            {
                timesFailed.Add(currentTime);
                int amountTimesFailed = timesFailed.Count;
            }

            CountUntilNextMin(nextTime);
        }

        public void CountUntilNextMin(DateTime nextTime)
        {
            do
            {
                System.Threading.Thread.Sleep(5000);
            } while (DateTime.Now < nextTime);
            nextTime = nextTime.AddMinutes(5);
            RunCollection(nextTime);
        }
    }
}
