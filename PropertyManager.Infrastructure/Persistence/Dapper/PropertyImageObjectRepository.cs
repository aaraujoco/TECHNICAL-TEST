using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Common.Models;
using PropertyManager.Application.Exceptions;
using PropertyManager.Domain.Entities;

namespace PropertyManager.Infrastructure.Persistence.Dapper
{
    /// <summary>
    /// Implementation of the IPropertyImageObjectRepository interface for managing property data.
    /// </summary>
    public class PropertyImageObjectRepository : IPropertyImageObjectRepository
    {
        private readonly IUnitOfWork _uow;


        public PropertyImageObjectRepository(
            IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<int> AddPropertyImageAsync(UploadPropertyImageModel image)
        {
            try
            {
                var repoRead = _uow.GetReadRepository<PropertyImage>();

                using var memoryStream = new MemoryStream();
                await image.File.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);

                var parameters = new Dictionary<string, object?>();
                parameters.Add("@IdProperty", image.IdProperty);
                parameters.Add("@File", base64String);
                parameters.Add("@Enabled", true);
                var result = await repoRead.ExecuteSpListAsync("Create_PropertyImage_Async", parameters);

                var propertyImage = result.FirstOrDefault();

                _uow.SaveChanges();

                return propertyImage!.IdPropertyImage;
            }
            catch (Exception ex)
            {
                throw new DbContextException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(int idProperty)
        {
            try
            {
                var parameters = new Dictionary<string, object?>();
                parameters.Add("@IdProperty", idProperty);

                var repoRead = _uow.GetReadRepository<PropertyImage>();
                var result = await repoRead.ExecuteSpListAsync("Get_PropertyImages_By_PropertyId", parameters);

                return result;
            }
            catch (Exception ex)
            {
                throw new DbContextException(ex.Message, ex);
            }
        }
    }
}
