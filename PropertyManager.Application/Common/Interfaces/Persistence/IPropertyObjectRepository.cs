using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Entities;

namespace PropertyManager.Application.Common.Interfaces.Persistence;
/// <summary>
/// Interface for Property repository to manage CRUD operations.
/// </summary>
public interface IPropertyObjectRepository
    {
    /// <summary>
    /// Retrieves an property by their unique identifier.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the property.</param>
    /// <returns>A task that represents the asynchronous operation, containing the property.</returns>
    Task<PropertyManager.Domain.Entities.Property> GetPropertyByIdAsync(int propertyId);

    /// <summary>
    /// Get filtered properties.
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public Task<IEnumerable<PropertyManager.Domain.Entities.Property>> GetFilteredPropertiesAsync(GetPropertiesPaginatedQuery filters);

    Task<int> GetFilteredCountPropertiesAsync(GetPropertiesPaginatedQuery filters);
    /// <summary>
    /// Adds a new property to the database.
    /// </summary>
    /// <param name="property">The property to add.</param>
    /// <returns>A task that represents the asynchronous operation, containing the unique identifier of the newly created property.</returns>
    Task<int> AddPropertyAsync(PropertyManager.Domain.Entities.Property property);

    /// <summary>
    /// Updates an existing property in the database.
    /// </summary>
    /// <param name="property">The property with updated information.</param>
    /// <returns>A task that represents the asynchronous operation, indicating whether the update was successful.</returns>
    Task<bool> PropertyUpdateModel(PropertyManager.Domain.Entities.Property property);

    /// <summary>
    /// Deletes an property from the database by their unique identifier.
    /// </summary>
    /// <param name="propertyId">The unique identifier of the property to delete.</param>
    /// <returns>A task that represents the asynchronous operation, indicating whether the deletion was successful.</returns>
    Task<bool> DeletePropertyAsync(int propertyId);

    /// <summary>
    /// Updates price the an existing property in the database.
    /// </summary>
    /// <param name="propertyId">The property with updated information.</param>
    /// <param name="newPrice">The price property with updated information.</param>
    /// <param name="updatedBy">User with update price.</param>
    /// <returns>A task that represents the asynchronous operation, indicating whether the update was successful.</returns>
    Task<bool> ChangePropertyPriceAsync(int propertyId, decimal newPrice, string updatedBy);
}

