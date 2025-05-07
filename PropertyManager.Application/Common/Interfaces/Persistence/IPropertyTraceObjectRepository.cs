using PropertyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Interfaces.Persistence
{
    /// <summary>
    /// Defines contract for PropertyTrace repository.
    /// </summary>
    public interface IPropertyTraceObjectRepository
    {
        /// <summary>
        /// Inserts a new PropertyTrace record.
        /// </summary>
        Task<int> CreateAsync(PropertyTrace propertyTrace);

        /// <summary>
        /// Retrieves a list of PropertyTraces by property ID.
        /// </summary>
        Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(int idProperty);
    }
}
