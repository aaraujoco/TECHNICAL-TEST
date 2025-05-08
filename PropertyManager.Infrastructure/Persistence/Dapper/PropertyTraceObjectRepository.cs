using Microsoft.Data.SqlClient;
using PropertyManager.Application.Common.Interfaces.Persistence;
using PropertyManager.Application.Exceptions;
using PropertyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Infrastructure.Persistence.Dapper
{
    public class PropertyTraceObjectRepository : IPropertyTraceObjectRepository
    {
        private readonly IUnitOfWork _uow;

        public PropertyTraceObjectRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        /// <summary>
        /// Creates a new PropertyTrace record in the database.
        /// </summary>
        /// <param name="propertyTrace">The PropertyTrace object to be created.</param>
        /// <returns>The created PropertyTrace object with its IdPropertyTrace set.</returns>
        public async Task<int> CreateAsync(PropertyTrace propertyTrace)
        {
            try
            {
                var repoRead = _uow.GetReadRepository<PropertyTrace>();

                var parameters = new Dictionary<string, object?>();
                parameters.Add("@DateSale", propertyTrace.DateSale);
                parameters.Add("@Name", propertyTrace.Name);
                parameters.Add("@Value", propertyTrace.Value);
                parameters.Add("@Tax", propertyTrace.Tax);
                parameters.Add("@IdProperty", propertyTrace.IdProperty);
                parameters.Add("@CreatedBy", propertyTrace.CreatedBy ?? (object)DBNull.Value);

                var result = await repoRead.ExecuteSpListAsync("Create_PropertyTrace_Async", parameters);

                var propertyTraceIdentity = result.FirstOrDefault();

                _uow.SaveChanges();

                return propertyTraceIdentity!.IdPropertyTrace;
            }
            catch (Exception ex)
            {
                throw new DbContextException(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(int idProperty)
        {
            try
            {
                var parameters = new Dictionary<string, object?>();
                parameters.Add("@IdProperty", idProperty);

                var repoRead = _uow.GetReadRepository<PropertyTrace>();
                var result = await repoRead.ExecuteSpListAsync("GetPropertyTraces_By_PropertyId", parameters);

                return result;

            }
            catch (Exception ex)
            {
                throw new DbContextException(ex.Message, ex);
            }
        }
    }
}
