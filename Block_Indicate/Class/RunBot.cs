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
        public RunBot(TradeBot tradeBot, ApplicationDbContext context, string userId)
        {
            Run(userId, tradeBot);
        }
        public async Task Run(string userId, TradeBot tradeBotPass)
        {
            var startBot = Task.Run(async () => {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        TradeBot tradeBot = tradeBotPass;
                        DateTime nextTime = DateTime.Now;
                        int prevMaxResultId = db.Results.Max(r => r.Id);
                        Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
                        TradeBot tradeBotUpdate = db.TradeBots.Where(b => b.UniqueSetId == tradeBot.UniqueSetId && b.CustomerId == customer.Id).Single();
                        int numActiveTrades = 0;
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

                                            tradeBotUpdate.NumberOfActiveTrades++;
                                            db.Update(tradeBotUpdate);
                                            db.SaveChanges();
                                            numActiveTrades++;
                                            TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, userId, newResult);
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
                                        TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, userId, newResult);
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
                    }
                }
                catch (Exception e)
                {
                    var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                    botClient.SendTextMessageAsync(
                        chatId: 542294321,
                        text: "Run Bot Failure : Scan for New Trade\nException: " + e
                    );
                }
            });
        }
    }
}
