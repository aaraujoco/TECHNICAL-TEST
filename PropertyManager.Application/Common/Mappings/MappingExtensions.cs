using PropertyManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> items, int pageNumber, int pageSize, int count) where TDestination : class
            => await PaginatedList<TDestination>.CreateAsync(items, pageNumber, pageSize, count);
    }
}
