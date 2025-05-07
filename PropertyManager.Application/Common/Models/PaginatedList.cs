using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyManager.Application.Common.Models;

public sealed class PaginatedList<T>
{
    public List<T> Properties { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = pageSize > 0 ? (int)Math.Ceiling(count / (double)pageSize) : 1;
        TotalCount = count;
        Properties = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, int count)
    {

        return Task.FromResult(new PaginatedList<T>(source.ToList(), count, pageNumber, pageSize));
    }
}
