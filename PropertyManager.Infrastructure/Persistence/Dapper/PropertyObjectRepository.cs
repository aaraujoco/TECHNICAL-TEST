using Dapper;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Interfaces.Services;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Exceptions;
using PropertyManager.Application.Property.Queries;
using PropertyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PropertyManager.Infrastructure.Persistence.Dapper;

/// <summary>
/// Implementation of the IPropertyObjectRepository interface for managing property data.
/// </summary>
public class PropertyObjectRepository : IPropertyObjectRepository
{
    private readonly IUnitOfWork _uow;
    

    public PropertyObjectRepository(
        IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<int> AddPropertyAsync(Property property)
    {
        try
        {
            var repoRead = _uow.GetReadRepository<Property>();

            var parameters = new Dictionary<string, object?>();
            parameters.Add("@Name", property.Name);
            parameters.Add("@Address", property.Address);
            parameters.Add("@Price", property.Price);
            parameters.Add("@CodeInternal", property.CodeInternal);
            parameters.Add("@Year", property.Year);
            parameters.Add("@IdOwner", property.IdOwner);
            parameters.Add("@CreatedDate", DateTime.Now);
            parameters.Add("@CreatedBy", property.CreatedBy);

            var result = await repoRead.ExecuteSpListAsync("Create_Property_Async", parameters);

            var propertyIdentity = result.FirstOrDefault();

            _uow.SaveChanges();

            return propertyIdentity!.IdProperty;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public async Task<bool> ChangePropertyPriceAsync(int propertyId, decimal newPrice, string updatedBy)
    {
        try
        {
            var repoRead = _uow.GetReadRepository<Property>();

            var parameters = new Dictionary<string, object?>();
            parameters.Add("@IdProperty", propertyId);
            parameters.Add("@NewPrice", newPrice);
            parameters.Add("@UpdatedBy", updatedBy);
            parameters.Add("@UpdatedDate", DateTime.Now);

            var affectedRows = await repoRead.ExecuteSpAsync("Update_Property_Price", parameters);

            _uow.SaveChanges();

            return affectedRows > 0;

        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public Task<bool> DeletePropertyAsync(int propertyId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Property>> GetFilteredPropertiesAsync(GetPropertiesPaginatedQuery filters)
    {
        try
        {
            var parameters = new Dictionary<string, object?>();
            parameters.Add("@Name", filters.Name);
            parameters.Add("@Address", filters.Address);
            parameters.Add("@Price", filters.Price);
            parameters.Add("@CodeInternal", filters.CodeInternal);
            parameters.Add("@Year", filters.Year);
            parameters.Add("@Page", filters.Page);
            parameters.Add("@Size", filters.Size);

            var repoRead = _uow.GetReadRepository<Property>();
            var result = await repoRead.ExecuteSpListAsync("GetProperties_With_Filters", parameters);
            return result;

        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public async Task<int> GetFilteredCountPropertiesAsync(GetPropertiesPaginatedQuery filters)
    {
        try
        {
            var parameters = new Dictionary<string, object?>();
            parameters.Add("@Name", filters.Name);
            parameters.Add("@Address", filters.Address);
            parameters.Add("@Price", filters.Price);
            parameters.Add("@CodeInternal", filters.CodeInternal);
            parameters.Add("@Year", filters.Year);

            var repoRead = _uow.GetReadRepository<TotalPropertiesObject>();
            var result = await repoRead.ExecuteSpListAsync("GetPropertiesCount_With_Filters", parameters);
            return result.FirstOrDefault()!.TotalProperties;

        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public async Task<Property> GetPropertyByIdAsync(int propertyId)
    {
        try
        {
            var parameters = new Dictionary<string, object?>();
            parameters.Add("@IdProperty", propertyId);

            var repoRead = _uow.GetReadRepository<Property>();
            var result = await repoRead.ExecuteSpListAsync("Get_PropertyById", parameters);

            return result.FirstOrDefault()!;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }

    public async Task<bool> PropertyUpdateModel(Property property)
    {
        try
        {
            var repoRead = _uow.GetReadRepository<Property>();

            var parameters = new Dictionary<string, object?>();
            parameters.Add("@IdProperty", property.IdProperty);
            parameters.Add("@Name", property.Name);
            parameters.Add("@Address", property.Address);
            parameters.Add("@Price", property.Price);
            parameters.Add("@CodeInternal", property.CodeInternal);
            parameters.Add("@Year", property.Year);
            parameters.Add("@IdOwner", property.IdOwner);
            parameters.Add("@UpdatedBy", property.UpdatedBy);
            parameters.Add("@UpdatedDate", property.UpdatedDate);

            var affectedRows = await repoRead.ExecuteSpAsync("Update_Property_Async", parameters);

            _uow.SaveChanges();

            return affectedRows > 0;
        }
        catch (Exception ex)
        {
            throw new DbContextException(ex.Message, ex);
        }
    }
}

