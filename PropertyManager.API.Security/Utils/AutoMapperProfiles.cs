using AutoMapper;
using PropertyManager.API.Security.Domain;
using PropertyManager.API.Security.Domain.Model;

namespace PropertyManager.API.Security.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserModel>();
        }
    }
}
