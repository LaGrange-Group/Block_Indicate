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
                        List<Result> results = db.Results.Where(r => r.BitcoinVolumeOriginal > 200 && r.RealTime.Hour >= 3 && r.RealTime.Hour <= 9 && r.Symbol != "HOTBTC" && r.LastPrice > 0.00000140m).ToList();
                        int prevMaxResultId = results.Max(r => r.Id);
                        Customer customer = db.Customers.Where(c => c.Id == customerId).Single();
                        TradeBot tradeBotUpdate = db.TradeBots.Where(b => b.UniqueSetId == tradeBot.UniqueSetId && b.CustomerId == customer.Id).Single();
                        int numActiveTrades = 0;
                        while (active)
                        {
                            active = CheckStatus(tradeBot.UniqueSetId);
                            if (DateTime.Now > nextTime)
                            {
                                results = db.Results.Where(r => r.BitcoinVolumeOriginal > 200 && r.RealTime.Hour >= 3 && r.RealTime.Hour <= 9 && r.Symbol != "HOTBTC" && r.LastPrice > 0.00000140m).ToList();
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
                                            TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, customerId, newResult);
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
                                        TradeBotContract botContract = new TradeBotContract(db, tradeBot, numActiveTrades, customerId, newResult);
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
                        var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                        await botClient.SendTextMessageAsync(
                            chatId: 542294321,
                            text: "Shut Down Bot: " + tradeBot.Name
                        );
                    }
                }
                catch (Exception e)
                {
                    var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
                    await botClient.SendTextMessageAsync(
                        chatId: 542294321,
                        text: "Run Bot Failure : Scan for New Trade\nException: " + e
                    );
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
