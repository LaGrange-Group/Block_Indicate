using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Block_Indicate.Class;
using Block_Indicate.Data;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Block_Indicate.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;

        public DashboardController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            NavPrice navPrice = new NavPrice();
            DateTime a = DateTime.Now;
            DateTime prevDay = new DateTime(a.Year, a.Month, a.Day - 1, a.Hour, a.Minute, 0);
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.CurrentPrices = navPrice.CurrentPrices();
            dashboard.BinanceBalances = ExchangeApiKeys.BinanceConnection == true ? ExchangeApiKeys.GetAccountBalances("Binance") : null;
            dashboard.HuobiBalances = ExchangeApiKeys.HuobiConnection == true ? ExchangeApiKeys.GetAccountBalances("Huobi") : null;
            dashboard.ValidDojiFourHoursBinance = db.TriggeredDojiFourHours.Where(d => d.RealTime > prevDay).ToList();
            dashboard.ValidDoubleVolumesBinance = db.ValidDoubleVolumeBinance.Where(d => d.RealTime > prevDay).ToList();
            dashboard.TradePerformance = db.TradePerformances.FirstOrDefault();
            return View(dashboard);
        }

    }
}