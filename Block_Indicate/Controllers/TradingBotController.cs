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
using Telegram.Bot;

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
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                NavPrice navPrice = new NavPrice();
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.BinanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.TotalBTC = Decimal.Round(Convert.ToDecimal(tradeView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                tradeView.Customer = customer;
                tradeView.TradeBots = db.TradeBots.Where(b => b.CustomerId == customer.Id).ToList();
                tradeView.CurrentPrices = navPrice.CurrentPrices();
                return View(tradeView);
            }
            catch
            {
                return Index();
            }

        }
        public IActionResult CreateNew()
        {
            //TradeBotViewModel tradeView = new TradeBotViewModel();
            //tradeView.TradeBot = new TradeBot();
            //tradeView.TradeBot.AllMarkets = true;
            //tradeView.TradeBot.AllocatedBitcoin = tradeView.TotalBTC;
            //tradeView.TradeBot.PercentTakeProfit = 2;
            //tradeView.TradeBot.PercentStopLoss = -10;
            //tradeView.TradeBot.Exchange = "Binance";
            //tradeView.TradeBot.Type = "All";
            //tradeView.TradeBot.NumberOfTrades = 1;
            //tradeView.TradeBot.Name = "Davids Bot";
            List<Tuple<string, decimal>> binanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
            ViewBag.TotalBTC = Decimal.Round(Convert.ToDecimal(binanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
            List<string> Exchanges = new List<string>() { "Binance", "Huobi" };
            List<string> Types = new List<string>() { "All", "Double Volume", "Four Hour Doji's" };
            List<int> NumberOfTrades = new List<int>() { 1, 2, 3, 4 };
            ViewBag.AllMarkets = true;
            ViewBag.Exchanges = new SelectList(Exchanges);
            ViewBag.Types = new SelectList(Types);
            ViewBag.NumberOfTrades = new SelectList(NumberOfTrades);
            return PartialView();

        }
        [HttpPost]
        public IActionResult CreateNew(TradeBot tradeBot)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
            List<TradeBot> tradeBots = db.TradeBots.Where(b => b.CustomerId == customer.Id).ToList();
            // Verify okay to start bot
            // - Bitcoin Enough for amount of trades
            // - AllMarkets True
            tradeBot.CustomerId = customer.Id;
            tradeBot.Status = true;
            tradeBot.UniqueSetId = tradeBots.Count > 0 ? tradeBots.Count + 1 : 1;
            db.TradeBots.Add(tradeBot);
            db.SaveChanges();
            RunBot runBot = new RunBot(tradeBot, tradeBot.CustomerId);
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            botClient.SendTextMessageAsync(
              chatId: 542294321,
              text: "Created New Bot " + tradeBot.Name
            );
            return RedirectToAction("Index");
        }
        public IActionResult ActiveTrades()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                NavPrice navPrice = new NavPrice();
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.BinanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.Customer = customer;
                tradeView.CurrentPrices = navPrice.CurrentPrices();
                return View(tradeView);
            }
            catch
            {
                return Index();
            }
        }
        public IActionResult HistoricalTrades()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                NavPrice navPrice = new NavPrice();
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.BinanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.Customer = customer;
                tradeView.CurrentPrices = navPrice.CurrentPrices();
                return View(tradeView);
            }
            catch
            {
                return Index();
            }
            return View();
        }

        public IActionResult DeleteBot(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            if (bot.Status == true)
            {
                ShutOffBot(botId);
            }
            db.TradeBots.Remove(bot);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void ShutOffBot(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            bot.Status = !bot.Status;
            db.Update(bot);
            db.SaveChanges();
        }
        public IActionResult SwitchBotPower(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            bot.Status = !bot.Status;
            db.Update(bot);
            db.SaveChanges();
            if (bot.Status == true)
            {
                StartExistingBot(bot);
            }
            return RedirectToAction("Index");
        }

        private void StartExistingBot(TradeBot tradeBot)
        {
            RunBot runBot = new RunBot(tradeBot, tradeBot.CustomerId);
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            botClient.SendTextMessageAsync(
              chatId: 542294321,
              text: "Started Bot " + tradeBot.Name
            );
        }

    }
}