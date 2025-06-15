using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Data;
using ticketSystem.Models;

[Authorize(Roles = "Admin,Manager")]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;
    public CategoriesController(ApplicationDbContext context) => _context = context;

    public async Task<IActionResult> Index()
        => View(await _context.Categories.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category cat)
    {
        if (!ModelState.IsValid) return View(cat);
        _context.Add(cat);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
        => id == null
            ? NotFound()
            : View(await _context.Categories.FindAsync(id));

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category cat)
    {
        if (id != cat.Id) return NotFound();
        if (!ModelState.IsValid) return View(cat);
        _context.Update(cat);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
        => id == null
            ? NotFound()
            : View(await _context.Categories.FirstOrDefaultAsync(c => c.Id == id));

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cat = await _context.Categories.FindAsync(id);
        _context.Categories.Remove(cat!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
