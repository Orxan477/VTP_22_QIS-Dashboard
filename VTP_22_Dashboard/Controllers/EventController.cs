using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.Event;

namespace VTP_22_Dashboard.Controllers
{
    public class EventController : Controller
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public EventController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index(EventVM eventVM)
        {
            DateTime date = DateTime.Parse("01.01.0001 00:00:00");
            if (eventVM.Filter.Date == date)
            {
                eventVM.Filter = DateTime.Now;
                EventVM evCurrent = new EventVM()
                {
                    Events = _context.Events.Where(x => x.Date.Date <= eventVM.Filter.Date && !x.IsActive)
                                            .Include(x => x.Departments).OrderByDescending(x => x.Date).ToList(),
                    Filter = DateTime.Now.Date
                };
                return View(evCurrent);
            }
            EventVM ev = new EventVM()
            {
                Events = _context.Events.Where(x => x.Date.Date == eventVM.Filter.Date && !x.IsActive)
                                            .Include(x => x.Departments).OrderByDescending(x => x.Date).ToList(),
                Filter = DateTime.Now.Date
            };
            return View(ev);
        }
        public async Task<IActionResult> Detail(int id)
        {
            EventVM ev = new EventVM()
            {
                Event = await _context.Events.Where(x => !x.IsActive && x.Id == id).Include(x => x.Departments).FirstOrDefaultAsync(),
            };
            if (ev is null) return NotFound();
            return View(ev);

        }
        public async Task<IActionResult> Create()
        {
            await GetSelectedItemAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventVM createEvent)
        {
            if (!ModelState.IsValid)
            {
                await GetSelectedItemAsync();
                return View();
            }
            Event ev = _mapper.Map<Event>(createEvent);
            await _context.Events.AddAsync(ev);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Event dbEv = await _context.Events.Where(x => !x.IsActive && x.Id == id).FirstOrDefaultAsync();
            if (dbEv is null) return NotFound();
            UpdateEventVM ev = _mapper.Map<UpdateEventVM>(dbEv);
            await GetSelectedItemAsync();
            return View(ev);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateEventVM updateEvent)
        {
            if (!ModelState.IsValid)
            {
                await GetSelectedItemAsync();
                return View();
            }
            Event ev = await _context.Events.Where(x => !x.IsActive && x.Id == id).FirstOrDefaultAsync();
            if (ev is null) return NotFound();
            UpdateDataEvent(ev, updateEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Event dbEv = await _context.Events.Where(x => !x.IsActive && x.Id == id).FirstOrDefaultAsync();
            if (dbEv is null) return NotFound();
            _context.Events.Remove(dbEv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> IsActive(int id)
        {
            Event dbEv = await _context.Events.Where(x => !x.IsActive && x.Id == id).FirstOrDefaultAsync();
            if (dbEv is null) return NotFound();
            dbEv.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private void UpdateDataEvent(Event ev, UpdateEventVM updateEv)
        {
            ev.Name = updateEv.Name;
            ev.Description = updateEv.Description;
            ev.DepartmentsId = updateEv.DepartmentsId;
            ev.Date = updateEv.Date;
            ev.Location = updateEv.Location;
            ev.Link = updateEv.Link;
        }
        private async Task GetSelectedItemAsync()
        {
            ViewBag.department = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
        }
    }
}
