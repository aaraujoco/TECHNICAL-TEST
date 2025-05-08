using PropertyManager.Domain.Entities;

namespace PropertyManager.Application.Common.Interfaces.Persistence;
public interface IPropertyImageObjectRepository
{
    Task<int> AddPropertyImageAsync(PropertyImage image);
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(int idProperty);
}

