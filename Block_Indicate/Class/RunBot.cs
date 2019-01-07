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
        public void Run(TradeBot tradeBot)
        {
            var startBot = Task.Run(async () => {
                Debug.WriteLine("Started " + tradeBot.Name + " Trade Bot");
                DateTime nextTime = DateTime.Now;
                while (true)
                {
                    if (DateTime.Now > nextTime)
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            TradeBotContract botContract = new TradeBotContract();
                            Debug.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ Ran " + tradeBot.Name + " Trade Bot");
                        }
                        nextTime = nextTime.AddSeconds(10);
                    }
                }
            });
        }
    }
}
