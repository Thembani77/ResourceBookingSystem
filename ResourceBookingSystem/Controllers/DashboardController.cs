using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceBookingSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;

            var todaysBookings = await _context.Bookings
                .Include(b => b.Resource)
                .Where(b => b.StartTime.Date == today)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            return View(todaysBookings);
        }

    }
}
