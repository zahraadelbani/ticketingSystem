using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;
using ticketSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ticketSystem.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ticketsWithProjects = _context.Tickets.Include(t => t.Project);
            return View(await ticketsWithProjects.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null) return NotFound();

            // Load activity logs for this ticket
            var activityLogs = await _context.ActivityLogs
                .Where(log => log.TicketId == ticket.Id)
                .OrderByDescending(log => log.PerformedAt)
                .ToListAsync();

            ViewBag.ActivityLogs = activityLogs;

            return View(ticket);
        }


        [Authorize(Roles = "User,Admin,Manager")]
        public IActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,ProjectId")] Ticket ticket)
        {
            ticket.CreatedAt = System.DateTime.Now;
            ticket.CreatedBy = User.Identity.Name;

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusList = new SelectList(new[] { "Open", "In Progress", "Resolved", "Closed" });
            return View(ticket);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();
            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusList = new SelectList(new[] { "Open", "In Progress", "Resolved", "Closed" });
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,CreatedAt,CreatedBy,ProjectId,AssignedTo")] Ticket ticket)
        {
            if (id != ticket.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var originalTicket = await _context.Tickets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (originalTicket == null) return NotFound();

                    _context.Update(ticket);
                    await _context.SaveChangesAsync();

                    var logs = new List<ActivityLog>();
                    var username = User.Identity.Name ?? "System";
                    var now = DateTime.Now;

                    if (ticket.Title != originalTicket.Title)
                    {
                        logs.Add(new ActivityLog
                        {
                            TicketId = ticket.Id,
                            Action = $"Title changed from \"{originalTicket.Title}\" to \"{ticket.Title}\"",
                            PerformedBy = username,
                            PerformedAt = now
                        });
                    }

                    if (ticket.Description != originalTicket.Description)
                    {
                        logs.Add(new ActivityLog
                        {
                            TicketId = ticket.Id,
                            Action = "Description updated",
                            PerformedBy = username,
                            PerformedAt = now
                        });
                    }

                    if (ticket.Status != originalTicket.Status)
                    {
                        logs.Add(new ActivityLog
                        {
                            TicketId = ticket.Id,
                            Action = $"Status changed from \"{originalTicket.Status}\" to \"{ticket.Status}\"",
                            PerformedBy = username,
                            PerformedAt = now
                        });
                    }

                    if (ticket.AssignedTo != originalTicket.AssignedTo)
                    {
                        logs.Add(new ActivityLog
                        {
                            TicketId = ticket.Id,
                            Action = $"Assignment changed from \"{originalTicket.AssignedTo}\" to \"{ticket.AssignedTo}\"",
                            PerformedBy = username,
                            PerformedAt = now
                        });
                    }

                    if (ticket.ProjectId != originalTicket.ProjectId)
                    {
                        var oldProject = await _context.Projects.FindAsync(originalTicket.ProjectId);
                        var newProject = await _context.Projects.FindAsync(ticket.ProjectId);

                        logs.Add(new ActivityLog
                        {
                            TicketId = ticket.Id,
                            Action = $"Project changed from \"{oldProject?.Name}\" to \"{newProject?.Name}\"",
                            PerformedBy = username,
                            PerformedAt = now
                        });
                    }

                    if (logs.Any())
                    {
                        _context.ActivityLogs.AddRange(logs);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tickets.Any(t => t.Id == ticket.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProjectId = new SelectList(_context.Projects, "Id", "Name", ticket.ProjectId);
            return View(ticket);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var ticket = await _context.Tickets.Include(t => t.Project).FirstOrDefaultAsync(m => m.Id == id);
            return ticket == null ? NotFound() : View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null) _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
