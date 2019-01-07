using Binance.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataCollectionCore
{
    class TriggeredDojiFourScan
    {
        aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context db;
        public TriggeredDojiFourScan(aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context _context)
        {
            db = _context;
        }
        public void ScanTriggered4hrDoji()
        {
            // If exists in triggered table, skip.
            try
            {
                List<FourHourDojis> tokens = db.FourHourDojis.ToList();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (FourHourDojis token in tokens)
                {
                    
                    TriggeredDojiFourHours four = new TriggeredDojiFourHours();
                    bool isTriggered = false;
                    using (var client = new BinanceClient())
                    {
                        var data = client.GetKlines(token.Symbol, Binance.Net.Objects.KlineInterval.FourHour, null, null, 1);
                        var current = client.Get24HPrice(token.Symbol).Data;
                        var kline = data.Data[0];
                        decimal priceToMatch = db.FourHourDojis.Where(d => d.Symbol == token.Symbol).Select(d => d.PriceToMatch).Single();
                        decimal volumeToMatch = db.FourHourDojis.Where(d => d.Symbol == token.Symbol).Select(d => d.BitcoinVolumeToMatch).Single();
                        if (kline.High >= priceToMatch && kline.TakerBuyQuoteAssetVolume >= volumeToMatch && token.Logged == false)
                        {
                            isTriggered = true;
                            four.Symbol = token.Symbol;
                            four.Open = kline.Open;
                            four.Close = kline.Close;
                            four.High = kline.High;
                            four.Low = kline.Low;
                            four.LastPrice = current.LastPrice;
                            four.PercentageChange = Convert.ToDouble(Decimal.Round(current.PriceChangePercent, 2));
                            four.Vwap = current.WeightedAveragePrice;
                            four.Volume = kline.Volume;
                            four.BitcoinVolume = kline.TakerBuyQuoteAssetVolume;
                            four.RealTime = DateTime.Now;
                        }
                    }
                    if (isTriggered)
                    {
                        db.TriggeredDojiFourHours.Add(four);
                        token.Logged = true;
                        db.Update(token);
                        db.SaveChanges();
                        Console.WriteLine("---------------------------------------- Has Found Valid Doji");
                    }
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine("Scanned for Volume Increase With Price Increase");
            }
            catch
            {
                ScanTriggered4hrDoji();
            }
        }

        //public void WaitForNextTime(DateTime nextTime)
        //{
        //    do
        //    {
        //        System.Threading.Thread.Sleep(10000);
        //    } while (DateTime.Now < nextTime);
        //    nextTime = DateTime.Now.AddMinutes(2);
        //    ScanTriggered4hrDoji(nextTime);
        //}
    }
}
