using AutoMapper;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.Department;
using VTP_22_Dashboard.ViewModels.Participant;
using VTP_22_Dashboard.ViewModels.University;

namespace VTP_22_Dashboard.Profiles
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Universities, UpdateUniVM>();
            CreateMap<CreateUniVM, Universities>();
            CreateMap<CreateDVM, Departments>();
            CreateMap<Departments, UpdateDVM>();
            CreateMap<AppUser, UpdatePraticipant>();
            CreateMap<UpdatePraticipant, AppUser>();
            }
    }
}
