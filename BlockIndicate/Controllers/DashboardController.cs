using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockIndicate.Class;
using BlockIndicate.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlockIndicate.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.CurrentPrices = NavPrice.currentPrices;
            return View(dashboard);
        }
    }
}