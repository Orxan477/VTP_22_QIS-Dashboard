using System.ComponentModel.DataAnnotations;

namespace VTP_22_Dashboard.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int DepartmentsId { get; set; }
        [Required]
        public Departments Departments { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Location { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
    }
}
