using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace DataCollectionCore
{
    class Run
    {
        public async Task RunAsync()
        {
            await RunPrograms();
        }
        public async Task RunPrograms()
        {
            DateTime one = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 00, 00);
            DateTime two = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);
            DateTime three = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 22, 00, 00);
            DateTime four = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 02, 00, 00);
            DateTime five = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 06, 00, 00);
            DateTime six = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 00, 00);

            var insertDoji = Task.Run(async () => {
                while (true)
                {
                    DateTime a = DateTime.Now;
                    DateTime currentTime = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 0);
                    if (currentTime == one || currentTime == two || currentTime == three || currentTime == four || currentTime == five || currentTime == six)
                    {
                        using (var db = new aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context())
                        {
                            DojiScan dojiScan = new DojiScan(db);
                            dojiScan.Scan4hrDoji();
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Inserted New Dojis");
                        }
                    }
                }
            });
            var insertBinanceGeneralDV = Task.Run(async () => {
                Console.WriteLine("Starting General Insert DV Binance");
                DateTime nextTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now > nextTime)
                    {
                        using (var db = new aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context())
                        {
                            InsertBinanceDoubleVolume binanceDoubleVolume = new InsertBinanceDoubleVolume(db);
                            binanceDoubleVolume.InsertGeneral();
                            binanceDoubleVolume.InsertValidDoubleVolume();
                            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Inserted Binance General");
                        }
                        nextTime = nextTime.AddMinutes(2);
                    }
                }
            });
            var resultsScanAll = Task.Run(async () =>
            {
                Console.WriteLine("Starting Results Scan");
                DateTime nextTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now > nextTime)
                    {
                        using (var db = new aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context())
                        {
                            ResultsScan resultsScan = new ResultsScan(db);
                            resultsScan.CheckForResultDoubleVolumeBinance();
                            resultsScan.CheckForResultFourHourDojiBinance();
                            resultsScan.UpdateExistingResults();
                            resultsScan.PrintResults();
                        }
                        nextTime = nextTime.AddMinutes(5);
                    }
                }
            });
            var scanTriggeredFourDoji = Task.Run(async () =>
            {
                DateTime nextTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now > nextTime)
                    {
                        using (var db = new aspnetBlock_IndicateEDDD8D4EC69940FD8558960B75DAC603Context())
                        {
                            TriggeredDojiFourScan dojiTriggeredScan = new TriggeredDojiFourScan(db);
                            dojiTriggeredScan.ScanTriggered4hrDoji();
                        }
                        nextTime = nextTime.AddMinutes(2);
                    }
                }
            });

            while (true)
            {
                System.Threading.Thread.Sleep(120000);
            }
        }
    }
}
