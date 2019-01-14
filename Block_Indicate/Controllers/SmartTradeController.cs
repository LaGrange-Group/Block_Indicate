using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Block_Indicate.Class;
using Block_Indicate.Data;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Block_Indicate.Controllers
{
    public class SmartTradeController : Controller
    {
        private readonly ApplicationDbContext db;

        public SmartTradeController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
            NavPrice navPrice = new NavPrice();
            Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
            SmartTradeViewModel smartView = new SmartTradeViewModel();
            smartView.Customer = customer;
            smartView.BinanceBalances = await binanceBalancesAsync;
            smartView.EstimatedBTC = smartView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
            smartView.TotalBTC = Decimal.Round(Convert.ToDecimal(smartView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
            smartView.CurrentPrices = await currentPricesAsync;
            return View(smartView);
        }
    }
}