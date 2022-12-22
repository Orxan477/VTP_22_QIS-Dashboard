using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.ViewModels.Home;

namespace VTP_22_Dashboard.Controllers
{
        [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM home = new HomeVM()
            {
                Users = _context.Users.Where(x => !x.IsActive).Skip(1).Count(),
                Departaments = _context.Departments.Count(),
                Universities = _context.Universities.Count()
            };
            var date = DateTime.Today;
            return View(home);
        }
    }
}