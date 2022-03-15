using AutoMapper;
using ICICI.AppCode.Reops.Entities;
using ICICI.Models;
using ICICI.Models.ViewModel;
namespace ICICI.AppCode.Helper
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<EmailConfig, EmailSettings>();
        }
    }
}
