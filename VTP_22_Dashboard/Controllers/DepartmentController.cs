using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.Department;

namespace VTP_22_Dashboard.Controllers
{
    public class DepartmentController : Controller
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public DepartmentController(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            DepartmentVM department = new DepartmentVM()
            {
                Departments = _context.Departments.ToList(),
            };
            return View(department);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDVM createDepartment)
        {
            if (!ModelState.IsValid)
            {
                return View(createDepartment);
            }
            bool name = _context.Departments.Any(x => x.Name.ToLower().Trim() == createDepartment.Name.ToLower().Trim());
            if (name)
            {
                ModelState.AddModelError("Name", "This Department Name is available");
                return View(createDepartment);
            }
            Departments uni = _mapper.Map<Departments>(createDepartment);
            await _context.Departments.AddAsync(uni);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            Departments dbDepartment = _context.Departments.Where(x => x.Id == id).FirstOrDefault();
            if (dbDepartment is null) return NotFound();
            UpdateDVM uni = _mapper.Map<UpdateDVM>(dbDepartment);
            return View(uni);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateDVM updateDepartment)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDepartment);
            }
            Departments department= await _context.Departments.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (department is null) return NotFound();
            bool name = _context.Departments.Any(x => x.Name.ToLower().Trim() == updateDepartment.Name.ToLower().Trim());
            bool currentName = (department.Name.ToLower().Trim() == updateDepartment.Name.Trim().ToLower());
            if (name && !currentName)
            {
                ModelState.AddModelError("Name", "This Department Name is available");
                return View(updateDepartment);
            }
            if (name && currentName)
            {
                return RedirectToAction(nameof(Index));
            }
            department.Name = updateDepartment.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Departments dbDepartment = _context.Departments.Where(x => x.Id == id).FirstOrDefault();
            if (dbDepartment is null) return NotFound();
            _context.Departments.Remove(dbDepartment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
