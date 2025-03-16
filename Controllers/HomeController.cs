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

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            // Fetch the list of stations
            var stations = await _context.Stations.ToListAsync();

            // Pass the list of stations to the view
            ViewBag.Stations = stations;

            return View();
        }
    }
}
