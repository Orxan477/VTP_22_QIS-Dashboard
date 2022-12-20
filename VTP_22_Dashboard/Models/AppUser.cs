using Microsoft.AspNetCore.Identity;

namespace VTP_22_Dashboard.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
        public Departments Departments { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
