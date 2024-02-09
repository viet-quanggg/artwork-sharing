using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtworkSharing.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> Entities => DbContext.Set<T>();

        public DbContext DbContext { get; private set; }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Add(T entity)
             => DbContext.Add(entity);


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await DbContext.AddAsync(entity, cancellationToken);


        public void AddRange(IEnumerable<T> entities)
            => DbContext.AddRange(entities);


        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await DbContext.AddRangeAsync(entities, cancellationToken);


        public T Get(Expression<Func<T, bool>> expression)
            => Entities.FirstOrDefault(expression);


        public IEnumerable<T> GetAll()
            => Entities.AsEnumerable();


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
            => Entities.Where(expression).AsEnumerable();


        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await Entities.ToListAsync(cancellationToken);


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await Entities.Where(expression).ToListAsync(cancellationToken);


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await Entities.FirstOrDefaultAsync(expression, cancellationToken);


        public void Remove(T entity)
            => DbContext.Remove(entity);


        public void RemoveRange(IEnumerable<T> entities)
            => DbContext.RemoveRange(entities);


        public void Update(T entity)
            => DbContext.Update(entity);


        public void UpdateRange(IEnumerable<T> entities)
            => DbContext.UpdateRange(entities);
    }
}
