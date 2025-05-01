using Microsoft.AspNetCore.Mvc;

namespace ticketSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
