namespace VTP_22_Dashboard.Models
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<AppUser> AppUsers { get; set; }
    }
}