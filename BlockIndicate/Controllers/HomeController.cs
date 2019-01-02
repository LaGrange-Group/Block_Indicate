using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlockIndicate.Models;
using BlockIndicate.Class;
using Binance.Net;
using BlockIndicate.Data;
using System.Threading;

namespace BlockIndicate.Controllers
{

    public class HomeController : Controller
    {
        public ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return RedirectToAction("RetrieveData");
        }
        public IActionResult RetrieveData()
        {
            using (var client = new BinanceClient())
            {
                var data = client.GetKlines("ETHBTC", Binance.Net.Objects.KlineInterval.OneMinute, null, null, 2);
                var lastKline = data.Data[0];
            }
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
