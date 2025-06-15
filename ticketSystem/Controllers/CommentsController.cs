using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;
using ticketSystem.Models;
using System.Security.Claims;

namespace ticketSystem.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments/Index?ticketId=1
        public async Task<IActionResult> Index(int ticketId)
        {
            var comments = await _context.Comments
                .Where(c => c.TicketId == ticketId)
                .Include(c => c.User)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.TicketId = ticketId;
            return View(comments);
        }

        // GET: Comments/Create
        public IActionResult Create(int ticketId)
        {
            var comment = new Comment { TicketId = ticketId };
            return View(comment);
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,TicketId")] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            var user = await _userManager.GetUserAsync(User);

            comment.CreatedAt = DateTime.Now;
            comment.UserId = user?.Id;
            comment.User = user;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { ticketId = comment.TicketId });
        }
        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null) return NotFound();

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                int ticketId = comment.TicketId;
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { ticketId = ticketId });
            }
            return NotFound();
        }

    }
}
