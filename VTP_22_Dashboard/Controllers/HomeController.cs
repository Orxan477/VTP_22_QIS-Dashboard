using Microsoft.AspNetCore.Mvc;

namespace VTP_22_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}