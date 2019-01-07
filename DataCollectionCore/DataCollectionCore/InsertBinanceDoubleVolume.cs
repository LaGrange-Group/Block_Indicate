using Binance.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataCollectionCore
{
    class InsertBinanceDoubleVolume
    {
        aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context db;
        public InsertBinanceDoubleVolume(aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context _context)
        {
            db = _context;
        }
        public void InsertGeneral()
        {
            try
            {
                DateTime a = DateTime.Now;
                DateTime currentTime = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0);
                List<string> tokens = new List<string> { "ETHBTC", "LTCBTC", "BNBBTC", "NEOBTC", "GASBTC", "BTCUSDT", "MCOBTC", "WTCBTC", "LRCBTC", "QTUMBTC", "YOYOBTC", "OMGBTC", "ZRXBTC", "STRATBTC", "SNGLSBTC", "BQXBTC", "KNCBTC", "FUNBTC", "SNMBTC", "IOTABTC", "LINKBTC", "XVGBTC", "SALTBTC", "MDABTC", "MTLBTC", "SUBBTC", "EOSBTC", "SNTBTC", "ETCBTC", "MTHBTC", "ENGBTC", "DNTBTC", "ZECBTC", "BNTBTC", "ASTBTC", "DASHBTC", "OAXBTC", "BTGBTC", "EVXBTC", "REQBTC", "VIBBTC", "TRXBTC", "POWRBTC", "ARKBTC", "XRPBTC", "MODBTC", "ENJBTC", "STORJBTC", "KMDBTC", "RCNBTC", "NULSBTC", "RDNBTC", "XMRBTC", "DLTBTC", "AMBBTC", "BATBTC", "BCPTBTC", "ARNBTC", "GVTBTC", "CDTBTC", "GXSBTC", "POEBTC", "QSPBTC", "BTSBTC", "XZCBTC", "LSKBTC", "TNTBTC", "FUELBTC", "MANABTC", "BCDBTC", "DGDBTC", "ADXBTC", "ADABTC", "PPTBTC", "CMTBTC", "XLMBTC", "CNDBTC", "LENDBTC", "WABIBTC", "TNBBTC", "WAVESBTC", "GTOBTC", "ICXBTC", "OSTBTC", "ELFBTC", "AIONBTC", "NEBLBTC", "BRDBTC", "EDOBTC", "WINGSBTC", "NAVBTC", "LUNBTC", "APPCBTC", "VIBEBTC", "RLCBTC", "INSBTC", "PIVXBTC", "IOSTBTC", "STEEMBTC", "NANOBTC", "VIABTC", "BLZBTC", "AEBTC", "NCASHBTC", "POABTC", "ZILBTC", "ONTBTC", "STORMBTC", "XEMBTC", "WANBTC", "WPRBTC", "QLCBTC", "SYSBTC", "GRSBTC", "CLOAKBTC", "GNTBTC", "LOOMBTC", "REPBTC", "TUSDBTC", "ZENBTC", "SKYBTC", "CVCBTC", "THETABTC", "IOTXBTC", "QKCBTC", "AGIBTC", "NXSBTC", "DATABTC", "SCBTC", "NPXSBTC", "KEYBTC", "NASBTC", "MFTBTC", "DENTBTC", "ARDRBTC", "HOTBTC", "VETBTC", "DOCKBTC", "POLYBTC", "PHXBTC", "HCBTC", "GOBTC", "PAXBTC", "RVNBTC", "DCRBTC", "USDCBTC", "MITHBTC", "BCHABCBTC" };
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (string token in tokens)
                {
                    DoubleVolumeBinance doubleVolume = new DoubleVolumeBinance();
                    using (var client = new BinanceClient())
                    {
                        var current = client.Get24HPrice(token).Data;
                        doubleVolume.Symbol = current.Symbol;
                        doubleVolume.LastPrice = current.LastPrice;
                        doubleVolume.PercentageChange = Convert.ToDouble(Decimal.Round(current.PriceChangePercent, 2));
                        doubleVolume.BitcoinVolume = current.QuoteVolume;
                        doubleVolume.RealTime = currentTime;
                    }
                    db.DoubleVolumeBinance.Add(doubleVolume);
                    db.SaveChanges();
                }
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
            }
            catch
            {
                Console.WriteLine("Failed General DV Binance Insert");
                System.Threading.Thread.Sleep(60000);
                InsertGeneral();
            }

        }

        public void InsertValidDoubleVolume()
        {
            try
            {
                DateTime a = DateTime.Now;
                DateTime currentTimeCut = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0);
                DateTime currentTime = DateTime.Now;
                DateTime prevTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day - 1, currentTime.Hour, currentTime.Minute, 00);
                DoubleVolumeBinance doubleVolumeRow = db.DoubleVolumeBinance.Where(d => d.RealTime > prevTime).FirstOrDefault();
                DateTime timeToMatch = doubleVolumeRow.RealTime;
                List<DoubleVolumeBinance> doubleVolumeRows = db.DoubleVolumeBinance.Where(d => d.RealTime == timeToMatch).ToList();
                DateTime tempDay = DateTime.Now;
                DateTime prevDay = new DateTime(tempDay.Year, tempDay.Month, tempDay.Day - 1, tempDay.Hour, tempDay.Minute, 0);
                foreach (DoubleVolumeBinance row in doubleVolumeRows)
                {
                    bool isValid = false;
                    bool exists = false;
                    ValidDoubleVolumeBinance validDouble = new ValidDoubleVolumeBinance();
                    using (var client = new BinanceClient())
                    {
                        var current = client.Get24HPrice(row.Symbol).Data;
                        if (current.QuoteVolume >= row.BitcoinVolume * 2 && current.QuoteVolume > 5)
                        {
                            List<string> existingSymbols = db.ValidDoubleVolumeBinance.Where(d => d.RealTime > prevDay).Select(d => d.Symbol).ToList();
                            foreach (string symbol in existingSymbols)
                            {
                                if (symbol == row.Symbol)
                                {
                                    exists = true;
                                }
                            }
                            validDouble.Symbol = row.Symbol;
                            validDouble.LastPrice = current.LastPrice;
                            validDouble.Vwap = current.WeightedAveragePrice;
                            validDouble.PercentageChange = Convert.ToDouble(Decimal.Round(current.PriceChangePercent, 2));
                            validDouble.Volume = current.Volume;
                            validDouble.BitcoinVolume = current.QuoteVolume;
                            validDouble.RealTime = currentTimeCut;
                            validDouble.PrevRowId = row.Id;
                            isValid = true;
                        }
                    }
                    if (isValid == true && exists == false)
                    {
                        db.ValidDoubleVolumeBinance.Add(validDouble);
                        db.SaveChanges();
                        Console.WriteLine("---------------------------------------- Has Found Valid Double Volume");
                    }
                }
            }
            catch
            {
                Console.WriteLine("Failed Valid DV Binance Insert");
                System.Threading.Thread.Sleep(60000);
                InsertGeneral();
            }
        }
    }
}
