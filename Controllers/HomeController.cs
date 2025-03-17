using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayReservation.Data;
using RailwayReservation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RailwayReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly RailwayContext _context;

        public HomeController(RailwayContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            
            var stations = await _context.Stations.ToListAsync();

            
            ViewBag.Stations = stations;

            return View();
        }
    }
}
