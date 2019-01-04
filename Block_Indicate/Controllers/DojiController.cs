using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Block_Indicate.Controllers
{
    public class DojiController : Controller
    {
        public IActionResult Index()
        {
            
            return View();
        }


    }
}