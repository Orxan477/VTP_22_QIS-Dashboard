using Microsoft.AspNetCore.Mvc;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.University;

namespace VTP_22_Dashboard.Controllers
{
    public class UniversityController : Controller
    {
        private AppDbContext _context;

        public UniversityController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            UniversityVM uVM = new UniversityVM()
            {
                Universities= _context.Universities.ToList(),
            };
            return View(uVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUniVM createUni)
        {
            if (!ModelState.IsValid)
            {
                return View(createUni);
            }
            bool name = _context.Universities.Any(x => x.Name.ToLower().Trim() == createUni.Name.ToLower().Trim());
            if (name)
            {
                ModelState.AddModelError("Name", "This University Name is available");
                return View();
            }
            Universities uni = new Universities()
            {
                Name = createUni.Name,
            };
            await _context.Universities.AddAsync(uni);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
