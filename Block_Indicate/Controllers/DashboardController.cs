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
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;

        public DashboardController(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = null;
                if (customer.ConnectedBinance == true)
                {
                    binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                }
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                DateTime a = DateTime.Now;
                DateTime prevDay = new DateTime(a.Year, a.Month, a.Day - 1, a.Hour, a.Minute, 0);
                DashboardViewModel dashboard = new DashboardViewModel();
                dashboard.Customer = customer;
                dashboard.ActiveBots = db.TradeBots.Where(b => b.CustomerId == customer.Id && b.Status == true).ToList().Count;
                dashboard.BotsTotal = db.TradeBots.Where(b => b.CustomerId == customer.Id).ToList().Count;
                dashboard.ValidDojiFourHoursBinance = db.TriggeredDojiFourHours.Where(d => d.RealTime > prevDay).ToList();
                dashboard.ValidDoubleVolumesBinance = db.ValidDoubleVolumeBinance.Where(d => d.RealTime > prevDay).OrderByDescending(d => d.RealTime).ToList();
                dashboard.TradePerformance = db.TradePerformances.FirstOrDefault();
                dashboard.BinanceBalances = await binanceBalancesAsync;
                dashboard.TotalBTC = dashboard.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                //dashboard.HuobiBalances = ExchangeApiKeys.HuobiConnection == true ? ExchangeApiKeys.GetAccountBalances("Huobi") : null;
                dashboard.CurrentPrices = await currentPricesAsync;
                return View(dashboard);
            }
            catch
            {
                return await Index();
            }


        }
    }
}