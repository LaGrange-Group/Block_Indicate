using System;
using System.Collections.Generic;
using Binance.Net;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace DataCollectionCore
{
    public class DojiScan
    {
        aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context db;
        public DojiScan(aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context _context)
        {
            db = _context;
        }
        public void Scan4hrDoji()
        {
            Console.WriteLine("Attempting Doji Insert");
            try
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [FourHourDojis]");
                Dictionary<string, double> prices = NavPrice.currentPrices;
                decimal minPrice = Convert.ToDecimal(0.0000027);
                List<string> tokens = new List<string> { "ETHBTC", "LTCBTC", "BNBBTC", "NEOBTC", "GASBTC", "BTCUSDT", "MCOBTC", "WTCBTC", "LRCBTC", "QTUMBTC", "YOYOBTC", "OMGBTC", "ZRXBTC", "STRATBTC", "SNGLSBTC", "BQXBTC", "KNCBTC", "FUNBTC", "SNMBTC", "IOTABTC", "LINKBTC", "XVGBTC", "SALTBTC", "MDABTC", "MTLBTC", "SUBBTC", "EOSBTC", "SNTBTC", "ETCBTC", "MTHBTC", "ENGBTC", "DNTBTC", "ZECBTC", "BNTBTC", "ASTBTC", "DASHBTC", "OAXBTC", "BTGBTC", "EVXBTC", "REQBTC", "VIBBTC", "TRXBTC", "POWRBTC", "ARKBTC", "XRPBTC", "MODBTC", "ENJBTC", "STORJBTC", "KMDBTC", "RCNBTC", "NULSBTC", "RDNBTC", "XMRBTC", "DLTBTC", "AMBBTC", "BATBTC", "BCPTBTC", "ARNBTC", "GVTBTC", "CDTBTC", "GXSBTC", "POEBTC", "QSPBTC", "BTSBTC", "XZCBTC", "LSKBTC", "TNTBTC", "FUELBTC", "MANABTC", "BCDBTC", "DGDBTC", "ADXBTC", "ADABTC", "PPTBTC", "CMTBTC", "XLMBTC", "CNDBTC", "LENDBTC", "WABIBTC", "TNBBTC", "WAVESBTC", "GTOBTC", "ICXBTC", "OSTBTC", "ELFBTC", "AIONBTC", "NEBLBTC", "BRDBTC", "EDOBTC", "WINGSBTC", "NAVBTC", "LUNBTC", "APPCBTC", "VIBEBTC", "RLCBTC", "INSBTC", "PIVXBTC", "IOSTBTC", "STEEMBTC", "NANOBTC", "VIABTC", "BLZBTC", "AEBTC", "NCASHBTC", "POABTC", "ZILBTC", "ONTBTC", "STORMBTC", "XEMBTC", "WANBTC", "WPRBTC", "QLCBTC", "SYSBTC", "GRSBTC", "CLOAKBTC", "GNTBTC", "LOOMBTC", "REPBTC", "TUSDBTC", "ZENBTC", "SKYBTC", "CVCBTC", "THETABTC", "IOTXBTC", "QKCBTC", "AGIBTC", "NXSBTC", "DATABTC", "SCBTC", "NPXSBTC", "KEYBTC", "NASBTC", "MFTBTC", "DENTBTC", "ARDRBTC", "HOTBTC", "VETBTC", "DOCKBTC", "POLYBTC", "PHXBTC", "HCBTC", "GOBTC", "PAXBTC", "RVNBTC", "DCRBTC", "USDCBTC", "MITHBTC", "BCHABCBTC" };
                List<string> dojis = new List<string>();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (string token in tokens)
                {
                    FourHourDojis four = new FourHourDojis();
                    bool isDoji = false;
                    using (var client = new BinanceClient())
                    {
                        var data = client.GetKlines(token, Binance.Net.Objects.KlineInterval.FourHour, null, null, 2);
                        var kline = data.Data[0];
                        double percentDiff = Convert.ToDouble(((kline.Open - kline.Close) / kline.Close * 100));
                        if (percentDiff < 0.10 && percentDiff > -0.10 && kline.Low > minPrice && kline.TakerBuyQuoteAssetVolume > 5)
                        {
                            isDoji = true;
                            four.Symbol = token;
                            four.Open = kline.Open;
                            four.Close = kline.Close;
                            four.High = kline.High;
                            four.Low = kline.Low;
                            four.Volume = kline.Volume;
                            four.BitcoinVolume = kline.TakerBuyQuoteAssetVolume;
                            four.PriceToMatch = kline.Open > kline.Close ? kline.Open : kline.Close;
                            four.PriceToMatch = (Convert.ToDecimal(0.01) * four.PriceToMatch) + four.PriceToMatch;
                            four.BitcoinVolumeToMatch = (four.BitcoinVolume / 4) + four.BitcoinVolume;
                            four.RealTime = DateTime.Now;
                            four.Logged = false;
                        }
                    }
                    if (isDoji)
                    {
                        db.FourHourDojis.Add(four);
                        db.SaveChanges();
                    }
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine("Inserted All Four Hour Dojis");
                System.Threading.Thread.Sleep(120000);
                //WaitForNextTime(db);
            }
            catch
            {
                System.Threading.Thread.Sleep(60000);
                Scan4hrDoji();
            }
        }

        //public void WaitForNextTime(aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context db)
        //{
        //    System.Threading.Thread.Sleep(300000);
        //    while (true)
        //    {
        //        DateTime addTwo = DateTime.Now.AddMinutes(2);
        //        DateTime orginal = DateTime.Now;
        //        DateTime minusTwo = orginal.Add(new TimeSpan(-2, 0, 0));
        //        if (addTwo > one && minusTwo < one || addTwo > two && minusTwo < two || addTwo > three && minusTwo < three || addTwo > four && minusTwo < four || addTwo > five && minusTwo < five || addTwo > six && minusTwo < six)
        //        {
        //            Scan4hrDoji();
        //        }
        //        System.Threading.Thread.Sleep(30000);
        //    }
        //}
    }
}
