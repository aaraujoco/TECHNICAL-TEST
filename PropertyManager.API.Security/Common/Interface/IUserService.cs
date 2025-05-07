using PropertyManager.API.Security.Domain;

namespace PropertyManager.API.Security.Common.Interface
{
    public interface IUserService
    {
        Task<User?> GetUser();
    }
}
