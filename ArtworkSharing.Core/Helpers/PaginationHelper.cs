using ArtworkSharing.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtworkSharing.Core.Helpers;

public class PaginationHelper
{
    public static PaginatedResult BuildPaginatedResult<T, TDto>(IMapper? mapper, IQueryable<T> source, int pageSize,
        int pageIndex)
    {
        var total = source.Count();
        if (total == 0)
            return new PaginatedResult
            {
                PageIndex = 1,
                PageSize = pageSize,
                Data = new List<TDto>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };

        pageSize = Math.Max(1, pageSize);
        var lastPage = (int)Math.Ceiling((decimal)total / pageSize);
        lastPage = Math.Max(1, lastPage);
        pageIndex = Math.Min(pageIndex, lastPage);
        var isLastPage = pageIndex == lastPage;

        var paginatedResult = new PaginatedResult
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total
        };

        if (pageIndex > lastPage / 2)
        {
            var mod = total % pageSize;
            var skip = Math.Max((lastPage - pageIndex - 1) * pageSize + mod, 0);
            var take = isLastPage ? mod : pageSize;
            var reverse = source.Reverse();

            var res = reverse.Skip(skip).Take(take);
            paginatedResult.Data = mapper is null ? res.Reverse() : mapper.Map<TDto>(res.Reverse());
            return paginatedResult;
        }

        var results = source.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        paginatedResult.Data = results;
        paginatedResult.Data = mapper is null ? results : mapper.Map<TDto>(results);
        return paginatedResult;
    }
    public static PaginatedResult BuildPaginatedResultFullOptions<T>(
     IQueryable<T> source,
     int pageSize,
     int pageIndex,
     Expression<Func<T, bool>> filter = null,
     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
     params Expression<Func<T, object>>[] includeProperties)
     where T : class
    {
        IQueryable<T> query = source;

        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);

        if (orderBy != null)
            query = orderBy(query);

        var total = query.Count();
        if (total == 0)
            return new PaginatedResult
            {
                PageIndex = 1,
                PageSize = pageSize,
                Data = new List<T>(),
                LastPage = 1,
                IsLastPage = true,
                Total = total
            };

        pageSize = Math.Max(1, pageSize);
        var lastPage = (long)Math.Ceiling((decimal)total / pageSize);
        lastPage = Math.Max(1, lastPage);
        pageIndex = Math.Min(pageIndex, (int)lastPage);
        var isLastPage = pageIndex == lastPage;

        query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        var paginatedResult = new PaginatedResult
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            LastPage = lastPage,
            IsLastPage = isLastPage,
            Total = total,
            Data = query.ToList()
        };

        return paginatedResult;
    }

    public static PaginatedResult BuildPaginatedResult<T>(IQueryable<T> source, int pageSize, int pageIndex)
    {
        return BuildPaginatedResult<T, T>(null, source, pageSize, pageIndex);
    }
}