using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyManager.Application.Common.Models;

namespace PropertyManager.Application.Common.Interfaces.Services;
    public interface IPropertyTraceObjectService
    {
        Task<(bool result, List<PropertyTraceModelOut> propertyTraces, string errorMessage)> GetByPropertyTraceByIdAsync(int idProperty);
}

