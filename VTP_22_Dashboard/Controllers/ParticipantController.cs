using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
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
        public IActionResult Index()
        {
            ParticipantVM participantVM = new ParticipantVM()
            {
                Participant=_context.Users.Skip(1).Where(x => !x.IsActive).Include(x=>x.Departments).Include(x=>x.Universities).ToList(),
            };
            return View(participantVM);
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
