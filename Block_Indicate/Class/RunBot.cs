using Block_Indicate.Data;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class RunBot
    {
        private readonly ApplicationDbContext db;
        private TradeBot tradeBot;
        private int numActiveTrades;
        private int prevMaxResultId;
        private string binanceApikey;
        private string binanceApiSecret;
        public RunBot(TradeBot tradeBot, ApplicationDbContext context, string userId)
        {
            db = context;
            this.tradeBot = tradeBot;
            numActiveTrades = 0;
            prevMaxResultId = db.Results.Max(r => r.Id) - 1;
            Run(userId);
        }
        public async Task Run(string userId)
        {

            var startBot = Task.Run(async () => {
                DateTime nextTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now > nextTime)
                    {
                        int highestId = db.Results.Max(r => r.Id);
                        bool isNew = highestId > prevMaxResultId ? true : false;
                        List<Result> newResults;
                        Result newResult = new Result();
                        if (isNew)
                        {
                            newResults = db.Results.Where(r => r.Id > prevMaxResultId).ToList();
                            if (newResults.Count > 1)
                            {
                                decimal highestVolume = newResults.Max(r => r.BitcoinVolumeOriginal);
                                newResult = newResults.Where(r => r.BitcoinVolumeOriginal == highestVolume).Single();
                            }
                            else
                            {
                                newResult = newResults[0];
                            }
                            if (tradeBot.NumberOfTrades > 1)
                            {

                                var startTrade = Task.Run(async () => {
                                    using (var db = new ApplicationDbContext())
                                    {
                                        TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, userId, newResult);

                                    }
                                });
                            }
                            else
                            {
                                using (var db = new ApplicationDbContext())
                                {
                                    TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, userId, newResult);

                                }
                            }
                            prevMaxResultId = highestId;
                            numActiveTrades++;
                        }
                        Debug.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Ran Scan" + tradeBot.Name + " Trade Bot");
                        nextTime = nextTime.AddSeconds(10);
                    }
                }
            });
        }
    }
}
