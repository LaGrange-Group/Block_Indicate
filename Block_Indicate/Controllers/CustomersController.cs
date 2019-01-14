using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Block_Indicate.Data;
using Block_Indicate.Models;
using System.Security.Claims;
using Block_Indicate.Class;

namespace Block_Indicate.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext db;

        public CustomersController(ApplicationDbContext context)
        {
            db = context;
        }
      
        public async Task<IActionResult> Profile()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                CustomerViewModel customerView = new CustomerViewModel();
                customerView.Customer = customer;
                customerView.BinanceBalances = await binanceBalancesAsync;
                customerView.EstimatedBTC = customerView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                customerView.TotalBTC = Decimal.Round(Convert.ToDecimal(customerView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                customerView.CurrentPrices = await currentPricesAsync;
                return View(customerView);
            }
            catch
            {
                return await Profile();
            }

        }
        public async Task<IActionResult> Billing()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                CustomerViewModel customerView = new CustomerViewModel();
                customerView.Customer = customer;
                customerView.BinanceBalances = await binanceBalancesAsync;
                customerView.EstimatedBTC = customerView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                customerView.TotalBTC = Decimal.Round(Convert.ToDecimal(customerView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                customerView.CurrentPrices = await currentPricesAsync;
                return View(customerView);
            }
            catch
            {
                return await Billing();
            }
        }
        public async Task<IActionResult> Settings()
        {
            try
            {
                Task<List<Tuple<string, decimal>>> binanceBalancesAsync = ExchangeApiKeys.GetAccountBalancesAsync("Binance");
                NavPrice navPrice = new NavPrice();
                Task<Dictionary<string, double>> currentPricesAsync = navPrice.CurrentPricesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Include(c => c.ApplicationUser).Where(c => c.UserId == userId).Single();
                CustomerViewModel customerView = new CustomerViewModel();
                customerView.Customer = customer;
                customerView.BinanceBalances = await binanceBalancesAsync;
                customerView.EstimatedBTC = customerView.BinanceBalances.Where(b => b.Item1 == "EstimatedBTC").Select(b => b.Item2).Single();
                customerView.TotalBTC = Decimal.Round(Convert.ToDecimal(customerView.BinanceBalances.Where(b => b.Item1 == "BTC").Select(b => b.Item2).Single().ToString("0.###########")), 5);
                customerView.CurrentPrices = await currentPricesAsync;
                return View(customerView);
            }
            catch
            {
                return await Billing();
            }
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(db.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FirstName,LastName,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.UserId = userId;
                customer.CompletedSignUp = true;
                db.Add(customer);
                db.SaveChanges();
                db.Customers.Where(c => c.UserId == userId).Single();
                Notification notification = new Notification();
                notification.CustomerId = customer.Id;
                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("CreateNotifications");
            }
            ViewData["UserId"] = new SelectList(db.ApplicationUsers, "Id", "Id", customer.UserId);
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult CreateNotifications()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
            Notification notification = new Notification();
            notification.CustomerId = customer.Id;
            db.Notifications.Add(notification);
            db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

    }
}
