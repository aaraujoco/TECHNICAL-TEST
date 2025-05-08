using PropertyManager.Application.Common.Models;
using PropertyManager.Domain.Entities;

namespace PropertyManager.Application.Common.Interfaces.Persistence;
public interface IPropertyImageObjectRepository
{
    Task<int> AddPropertyImageAsync(UploadPropertyImageModel image);
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(int idProperty);
}

