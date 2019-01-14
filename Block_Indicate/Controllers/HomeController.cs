using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Block_Indicate.Models;
using Block_Indicate.Data;
using System.Security.Claims;
using Block_Indicate.Class;

namespace Block_Indicate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login", "/Identity/Account");
        }

        public async Task<IActionResult> IsCustomer()
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = _context.Customers.Where(c => c.UserId == userId).Single();
                if (customer.CompletedSignUp == true)
                {
                    ExchangeApiKeys.Set(_context, userId);
                    //await UpdateBalaces();
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Create", "Customers");
            }
            catch
            {
                return RedirectToAction("Create", "Customers");
            }
        }
        public async Task UpdateBalaces()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = _context.Customers.Where(c => c.UserId == userId).Single();
            DateTime lastDay = _context.AccountPerformances.Where(a => a.CustomerId == customer.Id).Select(a => a.DateUpdated).Single();
            lastDay = lastDay.AddDays(1);
            DateTime currentTime = DateTime.Now;
            if (currentTime > lastDay)
            {
                //Get current account balances
                //await _context.SaveChangesAsync();
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
