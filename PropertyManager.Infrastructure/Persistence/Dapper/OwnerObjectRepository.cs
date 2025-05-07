using Dapper;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Exceptions;
using PropertyManager.Domain.Entities;
using System.Data;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// Implementation of the IOwnerObjectRepository interface for managing Owner data.
/// </summary>
public class OwnerObjectRepository: IOwnerObjectRepository
{
    private readonly IUnitOfWork _uow;

    public OwnerObjectRepository(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> AddOwnerAsync(Owner owner)
    {
        try
        {
            var repoRead = _uow.GetReadRepository<Owner>();

            var parameters = new Dictionary<string, object?>();
            parameters.Add("@Name", owner.Name);
            parameters.Add("@Address", owner.Address);
            parameters.Add("@Photo", owner.Photo);
            parameters.Add("@Birthday", owner.Birthday);
            parameters.Add("@CreatedDate", DateTime.Now);
            parameters.Add("@CreatedBy", owner.CreatedBy);

            var result = await repoRead.ExecuteSpListAsync("Create_Owner_Async", parameters);

            var ownerIdentity = result.FirstOrDefault();

            _uow.SaveChanges();

            return ownerIdentity!.IdOwner;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public Task<bool> DeleteOwnerAsync(int ownerId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Owner>> GetAllOwnersAsync()
    {
        try
        {
            
            var repoRead = _uow.GetReadRepository<Owner>();
            var result = await repoRead.ExecuteSpListAsync("GetAll_Owners");
            return result;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public async Task<Owner> GetOwnerByIdAsync(int ownerId)
    {
        try
        {
            var parameters = new Dictionary<string, object?>();
            parameters.Add("@IdOwner", ownerId);

            var repoRead = _uow.GetReadRepository<Owner>();
            var result = await repoRead.ExecuteSpListAsync("GetOwnerById", parameters);
            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public Task<bool> UpdateOwnerAsync(Owner owner)
    {
        throw new NotImplementedException();
    }
}
