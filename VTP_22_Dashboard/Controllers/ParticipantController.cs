using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels;
using VTP_22_Dashboard.ViewModels.Participant;

namespace VTP_22_Dashboard.Controllers
{
    public class ParticipantController : Controller
    {
        private IMapper _mapper;
        private AppDbContext _context;

        public ParticipantController(AppDbContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.TakeCount = 6;
            var products = _context.Users.Skip((page - 1) * 6)
                                .Take(6).Where(x => !x.IsActive).Include(x => x.Departments).Include(x => x.Universities).ToList();

            var productsVM = GetProductList(products);
            int pageCount = GetPageCount(6);
            Paginate<ParticipantVM> model = new Paginate<ParticipantVM>(productsVM, page, pageCount);
            return View(model);
        }
        private int GetPageCount(int take)
        {
            var prodCount = _context.Users.Where(x => !x.IsActive).Count();
            return (int)Math.Ceiling((decimal)prodCount / take);
        }
        private List<ParticipantVM> GetProductList(List<AppUser> user)
        {
            List<ParticipantVM> model = new List<ParticipantVM>();
            foreach (var item in user)
            {
                ParticipantVM userList = _mapper.Map<ParticipantVM>(item);
                model.Add(userList);
            }
            return model;
        }
        public async Task<IActionResult> Update(string id)
        {
            AppUser dbUser = await _context.Users.Where(x => x.Id == id && !x.IsActive).Include(x => x.Departments).Include(x => x.Universities).FirstOrDefaultAsync();
            if (dbUser is null) return NotFound();
            UpdatePraticipant user = _mapper.Map<UpdatePraticipant>(dbUser);
            await GetSelectedItemAsync();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdatePraticipant updatePraticipant)
        {
            if (!ModelState.IsValid)
            {
                await GetSelectedItemAsync();
                return View(updatePraticipant);
            }
            AppUser user = await _context.Users.Where(x => x.Id == id && !x.IsActive).FirstOrDefaultAsync();
            if (user is null) return NotFound();
            //user = _mapper.Map<AppUser>(updatePraticipant);
            user.FullName = updatePraticipant.FullName;
            ChangeUserData(user, updatePraticipant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private void ChangeUserData(AppUser user,UpdatePraticipant praticipant)
        {
            user.UserName = praticipant.UserName;
            user.PhoneNumber = praticipant.PhoneNumber;
            user.Email = praticipant.Email;
            user.NormalizedEmail = praticipant.Email.ToUpper();
            user.DepartmentsId = praticipant.DepartmentsId;
            user.UniversitiesId = praticipant.UniversitiesId;
        }
        public IActionResult Delete(string id)
        {
            AppUser dbUser = _context.Users.Where(x => x.Id == id && !x.IsActive).FirstOrDefault();
            if (dbUser is null) return NotFound();
            dbUser.IsActive = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private async Task GetSelectedItemAsync()
        {
            ViewBag.university = new SelectList(await _context.Universities
                                                             .ToListAsync(), "Id", "Name");
            ViewBag.department = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
        }
    }
}
