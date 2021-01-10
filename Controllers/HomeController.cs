using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tomi_Ionel.Models;
using Microsoft.EntityFrameworkCore;
using Tomi_Ionel.Data;
using Tomi_Ionel.Models.ShopViewModel;


namespace Tomi_Ionel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;
        public HomeController(ShopContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Orders
            group order by order.OrderDate into dateGroup
            select new OrderGroup()
            {
                OrderDate = dateGroup.Key,
                InstrumentCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
       /* 
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        public IActionResult Index()
        {
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
        public IActionResult Chat()
        {
            return View();
        }
    }
}
