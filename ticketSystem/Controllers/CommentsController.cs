using Microsoft.AspNetCore.Mvc;
using ticketSystem.Models;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;

namespace ticketSystem.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments for a ticket
        public async Task<IActionResult> Index(int ticketId)
        {
            var comments = await _context.Comments
                .Where(c => c.TicketId == ticketId)
                .Include(c => c.User)
                .ToListAsync();

            ViewBag.TicketId = ticketId;
            return View(comments);
        }

        // GET: Create
        public IActionResult Create(int ticketId)
        {
            var comment = new Comment { TicketId = ticketId };
            return View(comment);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { ticketId = comment.TicketId });
            }
            return View(comment);
        }
    }
}
