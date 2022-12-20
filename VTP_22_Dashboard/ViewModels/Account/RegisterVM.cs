using System.ComponentModel.DataAnnotations;

namespace VTP_22_Dashboard.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public int UniversityId { get; set; }
        [Required]
        public int DepartentId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
