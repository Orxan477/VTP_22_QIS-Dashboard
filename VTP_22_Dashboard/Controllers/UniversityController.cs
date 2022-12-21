using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.University;

namespace VTP_22_Dashboard.Controllers
{
    public class UniversityController : Controller
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public UniversityController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            UniversityVM uVM = new UniversityVM()
            {
                Universities = _context.Universities.ToList(),
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
                return View(createUni);
            }
            Universities uni = _mapper.Map<Universities>(createUni);
            await _context.Universities.AddAsync(uni);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            Universities dbUni= _context.Universities.Where(x => x.Id == id).FirstOrDefault();
            if (dbUni is null) return NotFound();
            UpdateUniVM uni = _mapper.Map<UpdateUniVM>(dbUni);
            return View(uni);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateUniVM updateuni)
        {
            if (!ModelState.IsValid)
            {
                return View(updateuni);
            }
            Universities uni = await _context.Universities.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (uni is null) return NotFound();
            bool name = _context.Universities.Any(x => x.Name.ToLower().Trim() == updateuni.Name.ToLower().Trim());
            bool currentName = (uni.Name.ToLower().Trim() == updateuni.Name.Trim().ToLower());
            if (name && !currentName)
            {
                ModelState.AddModelError("Name", "This University Name is available");
                return View(updateuni);
            }
            if (name && currentName)
            {
                return RedirectToAction(nameof(Index));
            }
            uni.Name = updateuni.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
