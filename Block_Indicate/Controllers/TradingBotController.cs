using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Block_Indicate.Class;
using Block_Indicate.Data;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Block_Indicate.Controllers
{
    public class TradingBotController : Controller
    {
        private readonly ApplicationDbContext db;

        public TradingBotController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            NavPrice navPrice = new NavPrice();
            TradeBotViewModel tradeView = new TradeBotViewModel();
            tradeView.BinanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
            tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
            tradeView.TotalBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single();
            tradeView.Customer = customer;
            tradeView.CurrentPrices = navPrice.CurrentPrices();
            tradeView.TradeBot = new TradeBot();
            tradeView.TradeBot.AllMarkets = true;
            tradeView.TradeBot.AllocatedBitcoin = tradeView.TotalBTC;
            tradeView.TradeBot.PercentTakeProfit = 2;
            tradeView.TradeBot.PercentStopLoss = -10;
            tradeView.TradeBot.Exchange = "Binance";
            List<string> Exchanges = new List<string>() { "Binance", "Huobi" };
            List<string> Types = new List<string>() { "All", "Double Volume", "Four Hour Doji's" };
            ViewBag.Exchanges = new SelectList(Exchanges);
            ViewBag.Types = new SelectList(Types);
            return View(tradeView);
        }
        [HttpPost]
        public IActionResult Index(TradeBot tradeBot)
        {
            RunBot runBot = new RunBot();
            runBot.Run(tradeBot);
            return RedirectToAction("Index");
        }
    }
}