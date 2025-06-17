using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;
using ticketSystem.Models;

[Authorize(Roles = "Admin,Manager")]
public class DashboardController : Controller
{
    readonly ApplicationDbContext _ctx;
    public DashboardController(ApplicationDbContext ctx) => _ctx = ctx;

    public async Task<IActionResult> Index(
        int? projectId, int? categoryId, int? priorityId,
        string? status)    // renamed userId → assignedTo
    {
        var q = _ctx.Tickets
            .Include(t => t.Project)
            .Include(t => t.Category)
            .Include(t => t.Priority)
            .AsQueryable();

        if (projectId.HasValue) q = q.Where(t => t.ProjectId == projectId);
        if (categoryId.HasValue) q = q.Where(t => t.CategoryId == categoryId);
        if (priorityId.HasValue) q = q.Where(t => t.PriorityId == priorityId);
        if (!string.IsNullOrEmpty(status)) q = q.Where(t => t.Status == status);

        var tickets = await q
          .Select(t => new DashboardTicketDto
          {
              Id = t.Id,
              Title = t.Title,
              Status = t.Status!,
              CategoryName = t.Category!.Name,
              PriorityName = t.Priority!.Name,
              AssignedTo = t.CreatedBy
          })
          .ToListAsync();

        var byStatus = tickets
            .GroupBy(x => x.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        ViewBag.Projects = await _ctx.Projects.ToListAsync();
        ViewBag.Categories = await _ctx.Categories.ToListAsync();
        ViewBag.Priorities = await _ctx.Priorities.ToListAsync();
        ViewBag.Users = await _ctx.Users.ToListAsync();

        var vm = new DashboardViewModel
        {
            TotalTickets = tickets.Count(),
            TicketsByStatus = byStatus,
            FilteredTickets = tickets,
            SelectedProjectId = projectId,
            SelectedCategoryId = categoryId,
            SelectedPriorityId = priorityId,
            SelectedStatus = status,
            SelectedUserId = null  // repurpose for your string filter
        };

        return View(vm);
    }
}

