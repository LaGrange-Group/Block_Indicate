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
    public class ExchangesController : Controller
    {
        private readonly ApplicationDbContext db;

        public ExchangesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ApiConnectionViewModel apiInfo)
        {
            if (ExchangeApiKeys.AttemptConnection(apiInfo.ApiKey, apiInfo.ApiSecrect, apiInfo.Exchange))
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
                if (apiInfo.Exchange == "Binance")
                {
                    customer.BinanceApiKey = apiInfo.ApiKey;
                    customer.BinanceApiSecret = apiInfo.ApiSecrect;
                    customer.ConnectedBinance = true;
                }
                else
                {
                    customer.HuobiApiKey = apiInfo.ApiKey;
                    customer.HuobiApiSecret = apiInfo.ApiSecrect;
                    customer.ConnectedHuobi = true;
                }

                db.Update(customer);
                db.SaveChanges();
            }
            return View();
        }
    }
}