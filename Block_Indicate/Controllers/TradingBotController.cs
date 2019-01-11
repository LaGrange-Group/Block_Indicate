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
        public async Task<IActionResult> Index()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.Customer = customer;
                tradeView.TradeBots = db.TradeBots.Where(b => b.CustomerId == customer.Id).ToList();
                tradeView.BinanceBalances = await binanceBalancesAsync;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.TotalBTC = Decimal.Round(Convert.ToDecimal(tradeView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                tradeView.CurrentPrices = await currentPricesAsync;
                return View(tradeView);
            }
            catch
            {
                return await Index();
            }

        }
        public async Task<IActionResult> CreateNew()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                List<string> Exchanges = new List<string>() { "Binance", "Huobi" };
                List<string> Types = new List<string>() { "All", "Double Volume", "Four Hour Doji's" };
                List<int> NumberOfTrades = new List<int>() { 1, 2, 3, 4 };
                ViewBag.AllMarkets = true;
                ViewBag.Exchanges = new SelectList(Exchanges);
                ViewBag.Types = new SelectList(Types);
                ViewBag.NumberOfTrades = new SelectList(NumberOfTrades);
                List<Tuple<string, decimal>> binanceBalances = await binanceBalancesAsync;
                ViewBag.TotalBTC = Decimal.Round(Convert.ToDecimal(binanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                return PartialView();
            }
            catch
            {
                return await CreateNew();
            }
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
            tradeBot.Status = false;
            int maxId;
            try
            {
                maxId = db.TradeBots.Max(t => t.Id);
            }catch
            {
                maxId = 1;
            }
            tradeBot.UniqueSetId = maxId + 1;
            db.TradeBots.Add(tradeBot);
            db.SaveChanges();
            //RunBot runBot = new RunBot(tradeBot, tradeBot.CustomerId);
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            botClient.SendTextMessageAsync(
              chatId: 542294321,
              text: "Created New Bot " + tradeBot.Name
            );
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ActiveTrades()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                Task<bool> updatePriceAsync = navPrice.UpdateActiveTrades(customer.Id);
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.Customer = customer;
                tradeView.Trades = db.Trades.Where(t => t.Active == true).ToList();
                tradeView.BinanceBalances = await binanceBalancesAsync;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.CurrentPrices = await currentPricesAsync;
                bool success = await updatePriceAsync;
                return View(tradeView);
            }
            catch
            {
                return await ActiveTrades();
            }
        }
        public async Task<IActionResult> HistoricalTrades()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                Task<bool> updatePriceAsync = navPrice.UpdateActiveTrades(customer.Id);
                TradeBotViewModel tradeView = new TradeBotViewModel();
                tradeView.Customer = customer;
                tradeView.Trades = db.Trades.Where(t => t.Active == false).ToList();
                tradeView.BinanceBalances = await binanceBalancesAsync;
                tradeView.EstimatedBTC = tradeView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                tradeView.CurrentPrices = await currentPricesAsync;
                return View(tradeView);
            }
            catch
            {
                return await HistoricalTrades();
            }
        }

        public async Task<IActionResult> DeleteBot(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            if (bot.Status == true)
            {
                await ShutOffBot(botId);
            }
            db.TradeBots.Remove(bot);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private async Task ShutOffBot(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            bot.Status = !bot.Status;
            db.Update(bot);
            await db.SaveChangesAsync();
        }
        public async Task<IActionResult> SwitchBotPower(int botId)
        {
            TradeBot bot = db.TradeBots.Where(b => b.Id == botId).Single();
            bot.Status = !bot.Status;
            db.Update(bot);
            await db.SaveChangesAsync();
            if (bot.Status == true)
            {
                await StartExistingBot(bot);
            }
            return RedirectToAction("Index");
        }

        private async Task StartExistingBot(TradeBot tradeBot)
        {
            RunBot runBot = new RunBot(tradeBot, tradeBot.CustomerId);
            var botClient = new TelegramBotClient("742635812:AAHHN_UwKgvCWSo6H2fRTehdi2gb_Un55EA");
            await botClient.SendTextMessageAsync(
              chatId: 542294321,
              text: "Started Bot " + tradeBot.Name
            );
        }

        public async Task<IActionResult> SellMarket(int tradeId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            Trade trade = db.Trades.Where(t => t.Id == tradeId).Single();
            bool sell = await Sell.Market(trade.Symbol, trade.Amount, customer);
            if (sell == true)
            {
                trade.Active = false;
                db.Update(trade);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("ActiveTrades");
        }

    }
}