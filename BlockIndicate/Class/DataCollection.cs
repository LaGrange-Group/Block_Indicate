using System;
using System.Collections.Generic;
using Binance.Net;
using BlockIndicate.Data;
using BlockIndicate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlockIndicate.Class
{
    public class DataCollection : Controller
    {
        public ApplicationDbContext db;
        private int timeRan;
        public DataCollection()
        {
            timeRan = 0;
        }
        public DataCollection(ApplicationDbContext context)
        {
            db = context;
        }
        public void InsertData(DateTime nextTime)
        {
            List<string> tokens = new List<string> { "ETHBTC", "LTCBTC", "BNBBTC", "NEOBTC", "GASBTC", "BTCUSDT", "MCOBTC", "WTCBTC", "LRCBTC", "QTUMBTC", "YOYOBTC", "OMGBTC", "ZRXBTC", "STRATBTC", "SNGLSBTC", "BQXBTC", "KNCBTC", "FUNBTC", "SNMBTC", "IOTABTC", "LINKBTC", "XVGBTC", "SALTBTC", "MDABTC", "MTLBTC", "SUBBTC", "EOSBTC", "SNTBTC", "ETCBTC", "MTHBTC", "ENGBTC", "DNTBTC", "ZECBTC", "BNTBTC", "ASTBTC", "DASHBTC", "OAXBTC", "BTGBTC", "EVXBTC", "REQBTC", "VIBBTC", "TRXBTC", "POWRBTC", "ARKBTC", "XRPBTC", "MODBTC", "ENJBTC", "STORJBTC", "KMDBTC", "RCNBTC", "NULSBTC", "RDNBTC", "XMRBTC", "DLTBTC", "AMBBTC", "BATBTC", "BCPTBTC", "ARNBTC", "GVTBTC", "CDTBTC", "GXSBTC", "POEBTC", "QSPBTC", "BTSBTC", "XZCBTC", "LSKBTC", "TNTBTC", "FUELBTC", "MANABTC", "BCDBTC", "DGDBTC", "ADXBTC", "ADABTC", "PPTBTC", "CMTBTC", "XLMBTC", "CNDBTC", "LENDBTC", "WABIBTC", "TNBBTC", "WAVESBTC", "GTOBTC", "ICXBTC", "OSTBTC", "ELFBTC", "AIONBTC", "NEBLBTC", "BRDBTC", "EDOBTC", "WINGSBTC", "NAVBTC", "LUNBTC", "APPCBTC", "VIBEBTC", "RLCBTC", "INSBTC", "PIVXBTC", "IOSTBTC", "STEEMBTC", "NANOBTC", "VIABTC", "BLZBTC", "AEBTC", "NCASHBTC", "POABTC", "ZILBTC", "ONTBTC", "STORMBTC", "XEMBTC", "WANBTC", "WPRBTC", "QLCBTC", "SYSBTC", "GRSBTC", "CLOAKBTC", "GNTBTC", "LOOMBTC", "REPBTC", "TUSDBTC", "ZENBTC", "SKYBTC", "CVCBTC", "THETABTC", "IOTXBTC", "QKCBTC", "AGIBTC", "NXSBTC", "DATABTC", "SCBTC", "NPXSBTC", "KEYBTC", "NASBTC", "MFTBTC", "DENTBTC", "ARDRBTC", "HOTBTC", "VETBTC", "DOCKBTC", "POLYBTC", "PHXBTC", "HCBTC", "GOBTC", "PAXBTC", "RVNBTC", "DCRBTC", "USDCBTC", "MITHBTC", "BCHABCBTC" };

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            DateTime currentTime = DateTime.Now;
            foreach (string token in tokens)
            {
                BinanceData binance = new BinanceData();
                using (var client = new BinanceClient())
                {
                    var data = client.GetKlines(token, Binance.Net.Objects.KlineInterval.OneMinute, null, null, 2);
                    var kline = data.Data[0];
                    var currentData = client.Get24HPrice(token).Data;
                    binance.Symbol = token;
                    binance.Close = kline.Close;
                    binance.CloseTime = kline.CloseTime;
                    binance.High = kline.High;
                    binance.Low = kline.Low;
                    binance.Open = kline.Open;
                    binance.OpenTime = kline.OpenTime;
                    binance.QuoteAssetVolume = kline.QuoteAssetVolume;
                    binance.TakerBuyBaseAssetVolume = kline.TakerBuyBaseAssetVolume;
                    binance.TakerBuyQuoteAssetVolume = kline.TakerBuyQuoteAssetVolume;
                    binance.TradeCount = kline.TradeCount;
                    binance.Volume = kline.Volume;
                    binance.BitcoinVolume = currentData.QuoteVolume;
                    binance.PercentageChange = currentData.PriceChangePercent;
                    binance.RealTime = currentTime;
                }
                db.BinanceData.Add(binance);
                db.SaveChanges();
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            CountUntilNextMin(nextTime);
        }

        private void GetDataSave(string token)
        {
            BinanceData binance = new BinanceData();
            using (var client = new BinanceClient())
            {
                var data = client.GetKlines(token, Binance.Net.Objects.KlineInterval.OneMinute, null, null, 2);
                var kline = data.Data[0];
                var currentData = client.Get24HPrice(token).Data;
                binance.Symbol = token;
                binance.Close = kline.Close;
                binance.CloseTime = kline.CloseTime;
                binance.High = kline.High;
                binance.Low = kline.Low;
                binance.Open = kline.Open;
                binance.OpenTime = kline.OpenTime;
                binance.QuoteAssetVolume = kline.QuoteAssetVolume;
                binance.TakerBuyBaseAssetVolume = kline.TakerBuyBaseAssetVolume;
                binance.TakerBuyQuoteAssetVolume = kline.TakerBuyQuoteAssetVolume;
                binance.TradeCount = kline.TradeCount;
                binance.Volume = kline.Volume;
                binance.BitcoinVolume = currentData.QuoteVolume;
                binance.PercentageChange = currentData.PriceChangePercent;
                binance.RealTime = DateTime.Now;
            }
            using (var db = new ApplicationDbContext())
            {
                db.BinanceData.Add(binance);
                db.SaveChanges();
            }
        }
        public void CountUntilNextMin(DateTime nextTime)
        {
            do
            {
                System.Threading.Thread.Sleep(5000);
            } while (DateTime.Now < nextTime);
            nextTime = nextTime.AddMinutes(2);
            timeRan++;
            int suspectedRows = timeRan * 148;
            InsertData(nextTime);
        }
    }
}
