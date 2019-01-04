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
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            DashboardViewModel dashboard = new DashboardViewModel();
            dashboard.CurrentPrices = NavPrice.currentPrices;
            return View(dashboard);
        }

    }
}