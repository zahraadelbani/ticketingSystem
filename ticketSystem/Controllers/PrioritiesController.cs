using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data; // adjust namespace if needed
using ticketSystem.Models;

[Authorize(Roles = "Admin,Manager")]
public class PrioritiesController : Controller
{
    private readonly ApplicationDbContext _context;
    public PrioritiesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
        => View(await _context.Priorities.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Priority priority)
    {
        if (!ModelState.IsValid) return View(priority);
        _context.Add(priority);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
        => id == null
            ? NotFound()
            : View(await _context.Priorities.FindAsync(id));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Priority priority)
    {
        if (id != priority.Id) return NotFound();
        if (!ModelState.IsValid) return View(priority);
        _context.Update(priority);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
        => id == null
            ? NotFound()
            : View(await _context.Priorities.FirstOrDefaultAsync(p => p.Id == id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var priority = await _context.Priorities.FindAsync(id);
        if (priority != null)
        {
            _context.Priorities.Remove(priority);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
