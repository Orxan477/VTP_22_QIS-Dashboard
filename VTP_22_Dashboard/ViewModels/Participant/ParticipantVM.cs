using VTP_22_Dashboard.Models;

namespace VTP_22_Dashboard.ViewModels.Participant
{
    public class ParticipantVM
    {
        //public List<AppUser> Participant { get; set; }
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Department { get; set; }
        public string University { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
