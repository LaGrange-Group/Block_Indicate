using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using System.Diagnostics;
using Block_Indicate.Models;
using Block_Indicate.Data;
using Microsoft.EntityFrameworkCore;

namespace Block_Indicate.Class
{
    public class DojiScan
    {
        DateTime one = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 00);
        DateTime two = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);
        DateTime three = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 00, 00);
        DateTime four = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 02, 00, 00);
        DateTime five = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 06, 00, 00);
        DateTime six = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 00, 00);
        ApplicationDbContext db;
        public DojiScan(ApplicationDbContext _context)
        {
            db = _context;
        }
        public void Scan4hrDoji()
        {
            try
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [FourHourDojis]");
                //Dictionary<string, double> prices = NavPrice.currentPrices;
                decimal minPrice = Convert.ToDecimal(0.0000027);
                List<string> tokens = new List<string> { "ETHBTC", "LTCBTC", "BNBBTC", "NEOBTC", "GASBTC", "BTCUSDT", "MCOBTC", "WTCBTC", "LRCBTC", "QTUMBTC", "YOYOBTC", "OMGBTC", "ZRXBTC", "STRATBTC", "SNGLSBTC", "BQXBTC", "KNCBTC", "FUNBTC", "SNMBTC", "IOTABTC", "LINKBTC", "XVGBTC", "SALTBTC", "MDABTC", "MTLBTC", "SUBBTC", "EOSBTC", "SNTBTC", "ETCBTC", "MTHBTC", "ENGBTC", "DNTBTC", "ZECBTC", "BNTBTC", "ASTBTC", "DASHBTC", "OAXBTC", "BTGBTC", "EVXBTC", "REQBTC", "VIBBTC", "TRXBTC", "POWRBTC", "ARKBTC", "XRPBTC", "MODBTC", "ENJBTC", "STORJBTC", "KMDBTC", "RCNBTC", "NULSBTC", "RDNBTC", "XMRBTC", "DLTBTC", "AMBBTC", "BATBTC", "BCPTBTC", "ARNBTC", "GVTBTC", "CDTBTC", "GXSBTC", "POEBTC", "QSPBTC", "BTSBTC", "XZCBTC", "LSKBTC", "TNTBTC", "FUELBTC", "MANABTC", "BCDBTC", "DGDBTC", "ADXBTC", "ADABTC", "PPTBTC", "CMTBTC", "XLMBTC", "CNDBTC", "LENDBTC", "WABIBTC", "TNBBTC", "WAVESBTC", "GTOBTC", "ICXBTC", "OSTBTC", "ELFBTC", "AIONBTC", "NEBLBTC", "BRDBTC", "EDOBTC", "WINGSBTC", "NAVBTC", "LUNBTC", "APPCBTC", "VIBEBTC", "RLCBTC", "INSBTC", "PIVXBTC", "IOSTBTC", "STEEMBTC", "NANOBTC", "VIABTC", "BLZBTC", "AEBTC", "NCASHBTC", "POABTC", "ZILBTC", "ONTBTC", "STORMBTC", "XEMBTC", "WANBTC", "WPRBTC", "QLCBTC", "SYSBTC", "GRSBTC", "CLOAKBTC", "GNTBTC", "LOOMBTC", "REPBTC", "TUSDBTC", "ZENBTC", "SKYBTC", "CVCBTC", "THETABTC", "IOTXBTC", "QKCBTC", "AGIBTC", "NXSBTC", "DATABTC", "SCBTC", "NPXSBTC", "KEYBTC", "NASBTC", "MFTBTC", "DENTBTC", "ARDRBTC", "HOTBTC", "VETBTC", "DOCKBTC", "POLYBTC", "PHXBTC", "HCBTC", "GOBTC", "PAXBTC", "RVNBTC", "DCRBTC", "USDCBTC", "MITHBTC", "BCHABCBTC" };
                List<string> dojis = new List<string>();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (string token in tokens)
                {
                    FourHourDoji four = new FourHourDoji();
                    bool isDoji = false;
                    using (var client = new BinanceClient())
                    {
                        var data = client.GetKlines(token, Binance.Net.Objects.KlineInterval.FourHour, null, null, 2);
                        var kline = data.Data[0];
                        double percentDiff = Convert.ToDouble(((kline.Open - kline.Close) / kline.Close * 100));
                        if (percentDiff < 0.10 && percentDiff > -0.10 && kline.Low > minPrice)
                        {
                            isDoji = true;
                            four.Symbol = token;
                            four.Open = kline.Open;
                            four.Close = kline.Close;
                            four.High = kline.High;
                            four.Low = kline.Low;
                            four.Volume = kline.Volume;
                            four.BitcoinVolume = kline.TakerBuyQuoteAssetVolume;
                            four.RealTime = DateTime.Now;
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
                WaitForNextTime(db);
            }
            catch
            {
                Scan4hrDoji();
            }
        }

        public void WaitForNextTime(ApplicationDbContext db)
        {
            System.Threading.Thread.Sleep(300000);
            while (true)
            {
                DateTime addTwo = DateTime.Now.AddMinutes(2);
                DateTime orginal = DateTime.Now;
                DateTime minusTwo = orginal.Add(new TimeSpan(-2, 0, 0));
                if (addTwo > one && minusTwo < one || addTwo > two && minusTwo < two || addTwo > three && minusTwo < three || addTwo > four && minusTwo < four || addTwo > five && minusTwo < five || addTwo > six && minusTwo < six)
                {
                    Scan4hrDoji();
                    return;
                }
                System.Threading.Thread.Sleep(30000);
            }
        }
    }
}
