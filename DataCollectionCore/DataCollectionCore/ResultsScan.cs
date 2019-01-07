using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Binance.Net;

namespace DataCollectionCore
{
    class ResultsScan
    {
        aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context db;
        public ResultsScan(aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context _context)
        {
            db = _context;
        }

        public void CheckForResultDoubleVolumeBinance()
        {
            try
            {
                //List<ValidDoubleVolumeBinance> tempRows = db.ValidDoubleVolumeBinance.ToList();
                //foreach (ValidDoubleVolumeBinance rowi in tempRows)
                //{
                //    rowi.Logged = false;
                //    db.Update(rowi);
                //    db.SaveChanges();
                //}
                List<ValidDoubleVolumeBinance> doubleVolumes = db.ValidDoubleVolumeBinance.Where(r => r.Logged == false).ToList();
                foreach (ValidDoubleVolumeBinance row in doubleVolumes)
                {
                    bool? result = null;
                    bool open = true;
                    using (var client = new BinanceClient())
                    {
                        var klines = client.GetKlines(row.Symbol, Binance.Net.Objects.KlineInterval.FiveMinutes, row.RealTime.AddHours(6), DateTime.Now.AddHours(6));
                        foreach (var kline in klines.Data)
                        {
                            if (kline.High >= ((row.LastPrice * Convert.ToDecimal(0.02)) + row.LastPrice))
                            {
                                result = true;
                                open = false;
                                break;
                            }
                            else if (kline.Low <= (row.LastPrice - (row.LastPrice * Convert.ToDecimal(0.10))))
                            {
                                result = false;
                                open = false;
                                break;
                            }
                        }
                    }

                    Results results = new Results();
                    results.Symbol = row.Symbol;
                    results.LastPrice = row.LastPrice;
                    results.Vwap = row.Vwap;
                    results.BitcoinVolumeOriginal = row.BitcoinVolume;
                    results.PrevRowId = row.Id;
                    results.ResultOfTrade = result;
                    results.Open = open;
                    if (result != null)
                    {
                        results.TimeToResult = (DateTime.Now - row.RealTime).ToString();
                    }
                    results.Type = "Double Volume";
                    results.Exchange = "Binance";
                    results.RealTime = row.RealTime;
                    db.Results.Add(results);
                    row.Logged = true;
                    db.Update(row);
                    db.SaveChanges();
                }
            }
            catch
            {
                Console.WriteLine("******************************************** Failed Insert New Results");

            }

        }

        public void CheckForResultFourHourDojiBinance()
        {
            try
            {
                //List<ValidDoubleVolumeBinance> tempRows = db.ValidDoubleVolumeBinance.ToList();
                //foreach (ValidDoubleVolumeBinance rowi in tempRows)
                //{
                //    rowi.Logged = false;
                //    db.Update(rowi);
                //    db.SaveChanges();
                //}
                List<TriggeredDojiFourHours> doubleVolumes = db.TriggeredDojiFourHours.Where(r => r.Logged == false).ToList();
                foreach (TriggeredDojiFourHours row in doubleVolumes)
                {
                    bool? result = null;
                    bool open = true;
                    using (var client = new BinanceClient())
                    {
                        var klines = client.GetKlines(row.Symbol, Binance.Net.Objects.KlineInterval.FiveMinutes, row.RealTime.AddHours(6), DateTime.Now.AddHours(6));
                        foreach (var kline in klines.Data)
                        {
                            if (kline.High >= ((row.LastPrice * Convert.ToDecimal(0.02)) + row.LastPrice))
                            {
                                result = true;
                                open = false;
                                break;
                            }
                            else if (kline.Low <= (row.LastPrice - (row.LastPrice * Convert.ToDecimal(0.10))))
                            {
                                result = false;
                                open = false;
                                break;
                            }
                        }
                    }

                    Results results = new Results();
                    results.Symbol = row.Symbol;
                    results.LastPrice = row.LastPrice;
                    results.Vwap = row.Vwap;
                    results.BitcoinVolumeOriginal = row.BitcoinVolume;
                    results.PrevRowId = row.Id;
                    results.ResultOfTrade = result;
                    results.Open = open;
                    if (result != null)
                    {
                        results.TimeToResult = (DateTime.Now - row.RealTime).ToString();
                    }
                    results.Type = "Four Hour Doji";
                    results.Exchange = "Binance";
                    results.RealTime = row.RealTime;
                    db.Results.Add(results);
                    row.Logged = true;
                    db.Update(row);
                    db.SaveChanges();
                }
            }
            catch
            {
                Console.WriteLine("******************************************** Failed Insert New Results");

            }

        }

        public void UpdateExistingResults()
        {
            List<Results> rows = db.Results.Where(r => r.Open == true).ToList();
            foreach (Results row in rows)
            {
                bool? result = null;
                bool open = true;
                DateTime a = DateTime.Now;
                DateTime prevDay = new DateTime(a.Year, a.Month, a.Day - 1, a.Hour, a.Minute, 0);
                if (row.RealTime > prevDay)
                {
                    using (var client = new BinanceClient())
                    {
                        var klines = client.GetKlines(row.Symbol, Binance.Net.Objects.KlineInterval.FiveMinutes, row.RealTime.AddHours(6), DateTime.Now.AddHours(6));
                        foreach (var kline in klines.Data)
                        {
                            if (kline.High >= ((row.LastPrice * Convert.ToDecimal(0.02)) + row.LastPrice))
                            {
                                result = true;
                                open = false;
                                break;
                            }
                            else if (kline.Low <= (row.LastPrice - (row.LastPrice * Convert.ToDecimal(0.10))))
                            {
                                result = false;
                                open = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    using (var client = new BinanceClient())
                    {
                        var current = client.Get24HPrice(row.Symbol).Data;
                        if (row.LastPrice > current.LastPrice)
                        {
                            row.NoTrade = true;
                            row.MaxPercentDiff24Hr = Decimal.Round(((current.LastPrice - row.LastPrice) / row.LastPrice) * 100, 2);
                        }
                        else if (row.LastPrice < current.LastPrice)
                        {
                            row.NoTrade = true;
                            row.MaxPercentDiff24Hr = Decimal.Round(((current.LastPrice - row.LastPrice) / row.LastPrice) * 100, 2);
                        }
                        open = false;
                        row.PercentageChangeAf = Convert.ToDouble(current.PriceChangePercent);
                        row.VolumeAf = current.Volume;
                        row.BitcoinVolumeAf = current.QuoteVolume;
                    }
                }
                row.Open = open;
                row.ResultOfTrade = result;
                if (result != null)
                {
                    row.TimeToResult = (DateTime.Now - row.RealTime).ToString();
                }
                db.Update(row);
                db.SaveChanges();
            }
            try
            {

            }
            catch
            {
                Console.WriteLine("******************************************** Failed Update Results");
            }

        }
        public void CheckForResultFourHourDoji()
        {

        }

        public void PrintResults()
        {

            List<Results> results = db.Results.ToList();
            double totalTradesDV = results.Where(r => r.Type == "Double Volume").ToList().Count;
            double totalSuccessDV = results.Where(r => r.ResultOfTrade == true && r.Type == "Double Volume").ToList().Count;
            double totalFailDV = results.Where(r => r.ResultOfTrade == false && r.Type == "Double Volume").ToList().Count;
            double totalOpenDV = results.Where(r => r.Open == true && r.Type == "Double Volume").ToList().Count;
            double totalNoTradeDV = results.Where(r => r.NoTrade == true && r.Type == "Double Volume").ToList().Count;
            double percentSuccessDV = (totalSuccessDV / totalTradesDV) * 100;
            double percentFailDV = (totalFailDV / totalTradesDV) * 100;
            double percentOpenDV = (totalOpenDV / totalTradesDV) * 100;
            double percentNoTradeDV = (totalNoTradeDV / totalTradesDV) * 100;
            Console.WriteLine("\nType: Double Volume");
            Console.WriteLine("Percent Success: % " + percentSuccessDV);
            Console.WriteLine("Percent Fail: % " + percentFailDV);
            Console.WriteLine("Percent Open: % " + percentOpenDV);
            Console.WriteLine("Percent No Trade: % " + percentNoTradeDV + "\n");

            double totalTrades4D = results.Where(r => r.Type == "Four Hour Doji").ToList().Count;
            double totalSuccess4D = results.Where(r => r.ResultOfTrade == true && r.Type == "Four Hour Doji").ToList().Count;
            double totalFail4D = results.Where(r => r.ResultOfTrade == false && r.Type == "Four Hour Doji").ToList().Count;
            double totalOpen4D = results.Where(r => r.Open == true && r.Type == "Four Hour Doji").ToList().Count;
            double totalNoTrade4D = results.Where(r => r.NoTrade == true && r.Type == "Four Hour Doji").ToList().Count;
            double percentSuccess4D = (totalSuccess4D / totalTrades4D) * 100;
            double percentFail4D = (totalFail4D / totalTrades4D) * 100;
            double percentOpen4D = (totalOpen4D / totalTrades4D) * 100;
            double percentNoTrade4D = (totalNoTrade4D / totalTrades4D) * 100;
            Console.WriteLine("\nType: Four Hour Doji");
            Console.WriteLine("Percent Success: % " + percentSuccess4D);
            Console.WriteLine("Percent Fail: % " + percentFail4D);
            Console.WriteLine("Percent Open: % " + percentOpen4D);
            Console.WriteLine("Percent No Trade: % " + percentNoTrade4D + "\n");

            totalTradesDV = results.Where(r => r.Type == "Double Volume" && r.Open == false).ToList().Count;
            totalSuccessDV = results.Where(r => r.ResultOfTrade == true && r.Type == "Double Volume").ToList().Count;
            totalFailDV = results.Where(r => r.ResultOfTrade == false && r.Type == "Double Volume").ToList().Count;
            percentSuccessDV = (totalSuccessDV / totalTradesDV) * 100;
            percentFailDV = (totalFailDV / totalTradesDV) * 100;


            totalTrades4D = results.Where(r => r.Type == "Four Hour Doji" && r.Open == false).ToList().Count;
            totalSuccess4D = results.Where(r => r.ResultOfTrade == true && r.Type == "Four Hour Doji").ToList().Count;
            totalFail4D = results.Where(r => r.ResultOfTrade == false && r.Type == "Four Hour Doji").ToList().Count;
            percentSuccess4D = (totalSuccess4D / totalTrades4D) * 100;
            percentFail4D = (totalFail4D / totalTrades4D) * 100;

            //List<decimal> noTradePercentsDoubleVolume = db.Results.Where(r => r.NoTrade == true && r.Type == "Double Volume").Select(r => r.MaxPercentDiff24Hr).ToList();
            //List<decimal> noTradePercentsFourHourDoji = db.Results.Where(r => r.NoTrade == true && r.Type == "Four Hour Doji").Select(r => r.MaxPercentDiff24Hr).ToList();
            //double noTradeAvgPercentDoubleVolume = Convert.ToDouble(noTradePercentsDoubleVolume.Average());
            //double noTradeAvgPercentFourHourDoji = Convert.ToDouble(noTradePercentsFourHourDoji.Average());

            //List<DateTime> tradeTimesDoubleVolume = db.Results.Where(r => r.Open == false && r.Type == "Double Volume").Select(r => Convert.ToDateTime(r.TimeToResult)).ToList();
            //List<DateTime> tradeTimesFourHourDoji = db.Results.Where(r => r.Open == false && r.Type == "Four Hour Doji").Select(r => Convert.ToDateTime(r.TimeToResult)).ToList();
            //double noTradeAvgPercentDoubleVolume = Convert.ToDouble(noTradePercentsDoubleVolume.Average());
            //double noTradeAvgPercentFourHourDoji = Convert.ToDouble(noTradePercentsFourHourDoji.Average());

            TradePerformances performances = db.TradePerformances.FirstOrDefault();
            performances.Dvbsuccess = percentSuccessDV;
            performances.Dvbfail = percentFailDV;
            performances.DvbavgTime = "Not Implemented";
            performances.DvbnoTrade = percentNoTradeDV;
            performances.DvbnoTradeAvgSettle = 1.0;
            performances.FourDbsuccess = percentSuccess4D;
            performances.FourDbfail = percentFail4D;
            performances.FourDbavgTime = "Not Implemented";
            performances.FourDbnoTrade = percentNoTrade4D;
            performances.FourDbnoTradeAvgSettle = 1.0;
            db.Update(performances);
            db.SaveChanges();

        }

    }
}
