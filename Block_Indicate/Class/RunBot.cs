using Block_Indicate.Data;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Block_Indicate.Class
{
    public class RunBot
    {
        public RunBot(TradeBot tradeBot, int custId)
        {
            Run(custId, tradeBot);
        }
        public async Task Run(int customerId, TradeBot tradeBotPass)
        {
            var startBot = Task.Run(async () => {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        bool active = true;
                        TradeBot tradeBot = tradeBotPass;
                        DateTime nextTime = DateTime.Now;
                        List<Result> results = db.Results.ToList();
                        int prevMaxResultId = results.Max(r => r.Id) - 1;
                        Customer customer = db.Customers.Where(c => c.Id == customerId).Single();
                        TradeBot tradeBotUpdate = db.TradeBots.Where(b => b.UniqueSetId == tradeBot.UniqueSetId && b.CustomerId == customer.Id).Single();
                        int numActiveTrades = 0;
                        while (active)
                        {
                            active = CheckStatus(tradeBot.UniqueSetId);
                            if (DateTime.Now > nextTime)
                            {
                                results = db.Results.ToList();
                                int highestId = results.Max(r => r.Id);
                                bool isNew = highestId > prevMaxResultId ? true : false;
                                List<Result> newResults;
                                Result newResult = new Result();
                                if (isNew)
                                {
                                    newResults = results.Where(r => r.Id > prevMaxResultId).ToList();
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

                                            tradeBotUpdate.NumberOfActiveTrades++;
                                            db.Update(tradeBotUpdate);
                                            db.SaveChanges();
                                            numActiveTrades++;
                                            TradeContract botContract = new TradeContract(db, tradeBot, numActiveTrades, customerId, newResult);
                                            numActiveTrades--;
                                            tradeBotUpdate.NumberOfActiveTrades--;
                                            db.Update(tradeBotUpdate);
                                            db.SaveChanges();

                                        });
                                    }
                                    else
                                    {

                                        tradeBotUpdate.NumberOfActiveTrades++;
                                        db.Update(tradeBotUpdate);
                                        db.SaveChanges();
                                        numActiveTrades++;
                                        TradeContract botContract = new TradeContract(db, tradeBot, numActiveTrades, customerId, newResult);
                                        numActiveTrades--;
                                        tradeBotUpdate.NumberOfActiveTrades--;
                                        db.Update(tradeBotUpdate);
                                        db.SaveChanges();

                                    }
                                    prevMaxResultId = highestId;
                                }
                                Debug.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Ran Scan" + tradeBot.Name + " Trade Bot");
                                nextTime = nextTime.AddSeconds(10);
                            }
                            System.Threading.Thread.Sleep(10000);
                        }
                        Notify notify = new Notify();
                        await notify.TelegramAsync(customerId, "Shut Down Bot " + tradeBot.Name);
                    }
                }
                catch (Exception e)
                {
                    Notify notify = new Notify();
                    await notify.TelegramAsync(customerId, "Run Bot Failure : Scan for New Trade\nException: " + e);
                }
            });
        }

        private bool CheckStatus(int uniqueId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    bool active = db.TradeBots.Where(b => b.UniqueSetId == uniqueId).Select(b => b.Status).Single();
                    return active;
                }
            }
            catch
            {
                return CheckStatus(uniqueId);
            }

        }
    }
}
