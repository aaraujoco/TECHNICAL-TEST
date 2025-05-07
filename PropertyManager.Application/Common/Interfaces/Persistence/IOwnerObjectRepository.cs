using PropertyManager.Domain.Entities;
using System;

namespace PropertyManager.Application.Common.Interfaces.Persistence;

/// <summary>
/// Interface for Owner repository to manage CRUD operations.
/// </summary>
public interface IOwnerObjectRepository
{
    /// <summary>
    /// Retrieves an owner by their unique identifier.
    /// </summary>
    /// <param name="ownerId">The unique identifier of the owner.</param>
    /// <returns>A task that represents the asynchronous operation, containing the owner.</returns>
    Task<Owner> GetOwnerByIdAsync(int ownerId);

    /// <summary>
    /// Retrieves all owners.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation, containing a list of owners.</returns>
    Task<IEnumerable<Owner>> GetAllOwnersAsync();

    /// <summary>
    /// Adds a new owner to the database.
    /// </summary>
    /// <param name="owner">The owner to add.</param>
    /// <returns>A task that represents the asynchronous operation, containing the unique identifier of the newly created owner.</returns>
    Task<int> AddOwnerAsync(Owner owner);

    /// <summary>
    /// Updates an existing owner in the database.
    /// </summary>
    /// <param name="owner">The owner with updated information.</param>
    /// <returns>A task that represents the asynchronous operation, indicating whether the update was successful.</returns>
    Task<bool> UpdateOwnerAsync(Owner owner);

    /// <summary>
    /// Deletes an owner from the database by their unique identifier.
    /// </summary>
    /// <param name="ownerId">The unique identifier of the owner to delete.</param>
    /// <returns>A task that represents the asynchronous operation, indicating whether the deletion was successful.</returns>
    Task<bool> DeleteOwnerAsync(int ownerId);
}