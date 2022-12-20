using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VTP_22_Dashboard.Models;

namespace VTP_22_Dashboard.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options){}

        public DbSet<Departments> Departments { get; set; }
    }
}
