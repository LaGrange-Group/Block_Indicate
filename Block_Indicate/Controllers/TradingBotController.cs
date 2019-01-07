﻿using System;
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
            List<int> NumberOfTrades = new List<int>() { 1, 2, 3, 4 };
            ViewBag.Exchanges = new SelectList(Exchanges);
            ViewBag.Types = new SelectList(Types);
            ViewBag.NumberOfTrades = new SelectList(Types);
            return View(tradeView);
        }
        [HttpPost]
        public IActionResult Index(TradeBot tradeBot)
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Verify okay to start bot
            // - Bitcoin Enough for amount of trades
            // - AllMarkets True
            RunBot runBot = new RunBot(tradeBot, db, userId);
            return RedirectToAction("Index");
        }
    }
}