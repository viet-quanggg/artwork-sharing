using ArtworkSharing.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL;

public class DbFactory : IDisposable
{
    private DbContext _dbContext;
    private bool _disposed;
    private readonly Func<ArtworkSharingContext> _instanceFunc;

    public DbFactory(Func<ArtworkSharingContext> dbContextFactory)
    {
        _instanceFunc = dbContextFactory;
    }

    public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

    public void Dispose()
    {
        if (!_disposed && _dbContext != null)
        {
            _disposed = true;
            _dbContext.Dispose();
        }
    }
}