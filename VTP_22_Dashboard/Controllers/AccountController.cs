using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.DAL;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.Utilities;
using VTP_22_Dashboard.ViewModels.Account;

namespace VTP_22_Dashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleManager<IdentityRole> _roleManager;
        public AccountController(AppDbContext context, 
                                 SignInManager<AppUser> signInManager, 
                                 UserManager<AppUser> userManager, 
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Register()
        {
            await GetSelectedItemAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View(register);

            AppUser newUser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UniversitiesId = register.UniversityId,
                DepartmentsId = register.DepartentId,
                PhoneNumber = register.PhoneNumber,
                UserName = register.UserName,
                CreatedAt=DateTime.UtcNow,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, "Admin123");
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                await GetSelectedItemAsync();
                return View();
            }
            //await _userManager.AddToRoleAsync(newUser, UserRoles.Admin.ToString());


            await _userManager.AddToRoleAsync(newUser, UserRoles.Student.ToString());
            await GetSelectedItemAsync();
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                return NotFound();
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task GetSelectedItemAsync()
        {
            ViewBag.university = new SelectList(await _context.Universities
                                                             .ToListAsync(), "Id", "Name");
            ViewBag.department = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name");
        }
        public async Task CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }
    }
}
