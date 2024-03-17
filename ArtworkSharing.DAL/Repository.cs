using System.Linq.Expressions;
using ArtworkSharing.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL;

public class Repository<T> : IRepository<T> where T : class
{
    public Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public DbSet<T> Entities => DbContext.Set<T>();

    public DbContext DbContext { get; }

    public void Add(T entity)
    {
        DbContext.Add(entity);
    }


    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.AddAsync(entity, cancellationToken);
    }


    public void AddRange(IEnumerable<T> entities)
    {
        DbContext.AddRange(entities);
    }


    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await DbContext.AddRangeAsync(entities, cancellationToken);
    }


    public T Get(Expression<Func<T, bool>> expression)
    {
        return Entities.FirstOrDefault(expression);
    }

    public virtual IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "",
        int? pageIndex = null, // Optional parameter for pagination (page number)
        int? pageSize = null) // Optional parameter for pagination (number of records per page)
    {
        IQueryable<T> query = Entities;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null) query = orderBy(query);

        // Implementing pagination
        if (pageIndex.HasValue && pageSize.HasValue)
        {
            // Ensure the pageIndex and pageSize are valid
            var validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
            var validPageSize =
                pageSize.Value > 0
                    ? pageSize.Value
                    : 10; // Assuming a default pageSize of 10 if an invalid value is passed

            query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
        }

        return query.ToList();
    }

    public IEnumerable<T> GetAll()
    {
        return Entities.AsEnumerable();
    }


    public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
    {
        return Entities.Where(expression).AsEnumerable();
    }


    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Entities.ToListAsync(cancellationToken);
    }


    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await Entities.Where(expression).ToListAsync(cancellationToken);
    }


    public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await Entities.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, bool saveChanges = true)
    {
        var entity = await Entities.FindAsync(id);
        await DeleteAsync(entity);

        if (saveChanges) await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity, bool saveChanges = true)
    {
        Entities.Remove(entity);
        if (saveChanges) await DbContext.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, bool saveChanges = true)
    {
        var enumerable = entities as T[] ?? entities.ToArray();
        if (enumerable.Any()) Entities.RemoveRange(enumerable);

        if (saveChanges) await DbContext.SaveChangesAsync();
    }

    public T Find(params object[] keyValues)
    {
        return Entities.Find(keyValues);
    }

    public virtual async Task<T?> FindAsync(params object[] keyValues)
    {
        return await Entities.FindAsync(keyValues);
    }
}