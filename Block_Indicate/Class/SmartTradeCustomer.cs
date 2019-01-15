using Block_Indicate.Data;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class SmartTradeCustomer
    {
        private BuildTrade build;
        public SmartTradeCustomer(BuildTrade build)
        {
            this.build = build;
        }
        public async Task<BuildTrade> Create()
        {
            build.Trade.CustomerId = build.Customer.Id;
            build.Trade.Exchange = "Binance";
            build.Trade.Type = "SmartTrade";
            build.Trade.RealTime = DateTime.Now;
            using (var db = new ApplicationDbContext())
            {
                db.Trades.Add(build.Trade);
                await db.SaveChangesAsync();
                return build;
            }
        }

        public async Task Update(BuildTrade build)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Update(build.Trade);
                await db.SaveChangesAsync();
            }
        }
    }
}
