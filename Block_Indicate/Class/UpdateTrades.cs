using Binance.Net;
using Block_Indicate.Data;
using Block_Indicate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Block_Indicate.Class
{
    public class UpdateTrades
    {
        public bool Update(int customerId)
        {
            List<Trade> trades;
            using (var db = new ApplicationDbContext())
            {
                trades = db.Trades.Where(t => t.Active == true && t.CustomerId == customerId).ToList();
                foreach (Trade trade in trades)
                {
                    using (var client = new BinanceClient())
                    {
                        DateTime a = DateTime.Now;
                        DateTime dateTime = new DateTime(a.Year, a.Month, a.Day, a.Hour, a.Minute, 00);
                        var current = client.Get24HPrice(trade.Symbol);
                        if (current.Success)
                        {
                            trade.CurrentPrice = current.Data.LastPrice;
                            trade.DiffFromStopLoss = trade.StopLoss - trade.BuyPrice;
                            trade.DiffFromTakeProfit = trade.TakeProfit - trade.BuyPrice;
                            trade.CurrentDiff = trade.CurrentPrice - trade.BuyPrice;
                            trade.CurrentPercentageResult = trade.CurrentPrice - trade.BuyPrice < 0 ? trade.CurrentDiff / trade.DiffFromStopLoss * 100 : trade.CurrentDiff / trade.DiffFromTakeProfit * 100;
                            trade.CurrentPercentageResult = trade.CurrentPercentageResult * 1m;
                            trade.TimeActive = (dateTime - trade.StartDate).ToString();
                        }
                    }
                    db.Update(trade);
                    db.SaveChanges();
                }
                return true;
            }
        }
    }
}
