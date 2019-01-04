using Bittrex.Net;
using BlockIndicate.Data;
using BlockIndicate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlockIndicate.Class
{
    public class BittrexDataCollection
    {
        public ApplicationDbContext db;
        private int timeRan;
        private List<DateTime> timesFailed = new List<DateTime>();
        List<double> times = new List<double>();
        public BittrexDataCollection()
        {
            timeRan = 0;
        }
        public BittrexDataCollection(ApplicationDbContext context)
        {
            db = context;
            timeRan = 0;
        }

        public void RunCollection(DateTime nextTime)
        {
            List<string> marketList = new List<string>() { "BTC-FLO", "BTC-NBT", "BTC-MUE", "BTC-XEM", "BTC-DMD", "BTC-GAM", "BTC-SPHR", "BTC-OK", "BTC-AEON", "BTC-ETH", "BTC-TX", "BTC-EXP", "BTC-AMP", "BTC-XLM", "BTC-RVR", "BTC-EMC", "BTC-FCT", "BTC-EGC", "BTC-SLS", "BTC-RADS", "BTC-DCR", "BTC-BSD", "BTC-XVG", "BTC-PIVX", "BTC-MEME", "BTC-STEEM", "BTC-2GIVE", "BTC-LSK", "BTC-BRK", "BTC-WAVES", "BTC-LBC", "BTC-SBD", "BTC-BRX", "BTC-ETC", "BTC-STRAT", "BTC-SYNX", "BTC-EBST", "BTC-VRM", "BTC-SEQ", "BTC-REP", "BTC-SHIFT", "BTC-ARDR", "BTC-XZC", "BTC-NEO", "BTC-ZEC", "BTC-ZCL", "BTC-IOP", "BTC-GOLOS", "BTC-UBQ", "BTC-KMD", "BTC-GBG", "BTC-SIB", "BTC-ION", "BTC-QWARK", "BTC-CRW", "BTC-SWT", "BTC-MLN", "BTC-ARK", "BTC-DYN", "BTC-TKS", "BTC-MUSIC", "BTC-DTB", "BTC-INCNT", "BTC-GBYTE", "BTC-GNT", "BTC-NXC", "BTC-EDG", "BTC-MORE", "BTC-WINGS", "BTC-RLC", "BTC-GNO", "BTC-GUP", "BTC-LUN", "BTC-HMQ", "BTC-ANT", "BTC-SC", "BTC-BAT", "BTC-ZEN", "BTC-QRL", "BTC-PTOY", "BTC-BNT", "BTC-NMR", "BTC-SNT", "BTC-DCT", "BTC-XEL", "BTC-MCO", "BTC-ADT", "BTC-PAY", "BTC-MTL", "BTC-STORJ", "BTC-ADX", "BTC-OMG", "BTC-CVC", "BTC-PART", "BTC-QTUM", "BTC-BCH", "BTC-DNT", "BTC-ADA", "BTC-MANA", "BTC-SALT", "BTC-TIX", "BTC-RCN", "BTC-VIB", "BTC-MER", "BTC-POWR", "BTC-ENG", "BTC-UKG", "BTC-IGNIS", "BTC-SRN", "BTC-WAX", "BTC-ZRX", "BTC-VEE", "BTC-BCPT", "BTC-TRX", "BTC-TUSD", "BTC-LRC", "BTC-UP", "BTC-DMT", "BTC-POLY", "BTC-PRO", "BTC-BLT", "BTC-STORM", "BTC-AID", "BTC-NGC", "BTC-GTO", "BTC-OCN", "BTC-TUBE", "BTC-CBC", "BTC-CMCT", "BTC-NLC2", "BTC-BKX", "BTC-MFT", "BTC-LOOM", "BTC-RFR", "BTC-RVN", "BTC-BFT", "BTC-GO", "BTC-HYDRO", "BTC-UPP", "BTC-ENJ", "BTC-MET", "BTC-DTA", "BTC-EDR", "BTC-BOXX", "BTC-IHT", "BTC-XHV", "BTC-PMA", "BTC-PAL", "BTC-PAX", "BTC-ZIL", "BTC-MOC", "BTC-OST", "BTC-SPC", "BTC-MEDX", "BTC-BSV", "BTC-IOST", "BTC-XNK", "BTC-NCASH", "BTC-SOLVE", "BTC-USDS", "BTC-JNT", "BTC-LBA", "BTC-MOBI" };
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DateTime currentTime = DateTime.Now;
            int amount = marketList.Count;
            foreach (string market in marketList)
            {
                BittrexData bittrex = new BittrexData();
                using (var client = new BittrexClient())
                {
                    var result = client.GetMarketSummary(market).Data;
                    bittrex.Symbol = result.MarketName;
                    bittrex.High = result.High;
                    bittrex.Low = result.Low;
                    bittrex.Volume = result.Volume.HasValue ? Decimal.Round(result.Volume.Value, 8) : 0;
                    bittrex.Last = result.Last;
                    bittrex.BaseVolume = result.BaseVolume;
                    bittrex.TimeStamp = result.TimeStamp;
                    bittrex.Bid = result.Bid;
                    bittrex.Ask = result.Ask;
                    bittrex.OpenBuyOrders = result.OpenBuyOrders;
                    bittrex.OpenSellOrders = result.OpenSellOrders;
                    bittrex.PrevDay = result.PrevDay;
                    bittrex.PercentageChange = Decimal.Round(((result.Last - result.PrevDay) / result.PrevDay * 100).Value, 2);
                    bittrex.RealTime = currentTime;
                }
                //db.BittrexData.Add(bittrex);
                //db.SaveChanges();
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            times.Add(ts.TotalMinutes);
            double averageTime = times.Average();
            timeRan++;
            CountUntilNextMin(nextTime);
        }
        public void CountUntilNextMin(DateTime nextTime)
        {
            do
            {
                System.Threading.Thread.Sleep(5000);
            } while (DateTime.Now < nextTime);
            nextTime = nextTime.AddMinutes(1);
            RunCollection(nextTime);
        }
    }
}
