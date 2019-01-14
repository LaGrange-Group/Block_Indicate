using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Block_Indicate.Data;
using Block_Indicate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Block_Indicate.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext db;

        public NotificationsController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TelegramNotifications()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
            Notification notification = db.Notifications.Where(n => n.CustomerId == customer.Id).Single();
            return PartialView(notification);
        }
        [HttpPost]
        public async Task<IActionResult> TelegramNotifications(Notification notification)
        {
            db.Update(notification);
            await db.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult EmailNotifications()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = db.Customers.Where(c => c.UserId == userId).Single();
            Notification notification = db.Notifications.Where(n => n.CustomerId == customer.Id).Single();
            return PartialView(notification);
        }
        [HttpPost]
        public async Task<IActionResult> EmailNotifications(Notification notification)
        {
            db.Update(notification);
            await db.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}