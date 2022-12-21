using AutoMapper;
using VTP_22_Dashboard.Models;
using VTP_22_Dashboard.ViewModels.University;

namespace VTP_22_Dashboard.Profiles
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Universities, UpdateUniVM>();
            CreateMap<CreateUniVM, Universities>();
        }
    }
}
