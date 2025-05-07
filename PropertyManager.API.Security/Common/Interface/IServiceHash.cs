using Microsoft.AspNetCore.Http.HttpResults;
using PropertyManager.API.Security.Domain;

namespace PropertyManager.API.Security.Common.Interface
{
    public interface IServiceHash
    {
        ResultHash Hash(string input);
        ResultHash Hash(string input, byte[] sal);
    }
}
