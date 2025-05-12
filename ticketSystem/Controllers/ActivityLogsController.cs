using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;
using ticketSystem.Models;

namespace ticketSystem.Controllers
{
    public class ActivityLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityLogs/Index?ticketId=1
        public async Task<IActionResult> Index(int ticketId)
        {
            var logs = await _context.ActivityLogs
                .Where(l => l.TicketId == ticketId)
                .OrderByDescending(l => l.PerformedAt)
                .ToListAsync();

            ViewBag.TicketId = ticketId;
            return View(logs);
        }
    }
}
