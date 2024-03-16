using System.Data;
using ArtworkSharing.Core.Interfaces;
using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ArtworkSharing.DAL;

public class UnitOfWork : IUnitOfWork
{
    private IsolationLevel? _isolationLevel;

    private IDbContextTransaction _transaction;

    public UnitOfWork(DbFactory dbFactory)
    {
        DbContext = dbFactory.DbContext;
        ArtworkRepository = new ArtworkRepository(DbContext);
        UserRepository = new UserRepository(DbContext);
        ArtworkPackageRepository = new ArtistPackageRepository(DbContext);
        ArtistRepository = new ArtistRepository(DbContext);
        ArtworkServiceRepository = new ArtworkServiceRepository(DbContext);
        CategoryRepository = new CategoryRepository(DbContext);
        CommentRepository = new CommentRepository(DbContext);
        FollowRepository = new FollowRepository(DbContext);
        LikeRepository = new LikeRepository(DbContext);
        MediaContentRepository = new MediaContentRepository(DbContext);
        PackageRepository = new PackageRepository(DbContext);
        RatingRepository = new RatingRepository(DbContext);
        RefundRequestRepository = new RefundRequestRepository(DbContext);
        TransactionRepository = new TransactionRepository(DbContext);
        
        VNPayTransactionRepository = new VNPayTransactionRepository(DbContext);
        VNPayTransactionRefundRepository = new VNPayTransactionRefundRepository(DbContext);
        VNPayTransactionTransferRepository = new VNPayTransactionTransferRepository(DbContext);
        UserRepository = new UserRepository(DbContext);
        UserRoleRepository = new UserRoleRepository(DbContext);

    }

    public DbContext DbContext { get; }

    public IUserRepository UserRepository { get; }

    public IArtworkRepository ArtworkRepository { get; }

    public IArtistPackageRepository ArtworkPackageRepository { get; }

    public IArtistRepository ArtistRepository { get; }

    public IArtworkServiceRepository ArtworkServiceRepository { get; }

    public ICategoryRepository CategoryRepository { get; }
    public ICommentRepository CommentRepository { get; }
    public IFollowRepository FollowRepository { get; }

    public ILikeRepository LikeRepository { get; }

    public IMediaContentRepository MediaContentRepository { get; }

    public IPackageRepository PackageRepository { get; }

    public IRatingRepository RatingRepository { get; }

    public IRefundRequestRepository RefundRequestRepository { get; }

    public ITransactionRepository TransactionRepository { get; }

    public IVNPayTransactionRepository VNPayTransactionRepository { get; }

    public IVNPayTransactionRefundRepository VNPayTransactionRefundRepository { get; }

    public IVNPayTransactionTransferRepository VNPayTransactionTransferRepository { get; }

    public IUserRoleRepository UserRoleRepository { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransaction()
    {
        await StartNewTransactionIfNeeded();
    }

    public async Task CommitTransaction()
    {
        /*
         do not open transaction here, because if during the request
         nothing was changed(only select queries were run), we don't
         want to open and commit an empty transaction -calling SaveChanges()
         on _transactionProvider will not send any sql to database in such case
        */
        await DbContext.SaveChangesAsync();

        if (_transaction == null) return;
        await _transaction.CommitAsync();

        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransaction()
    {
        if (_transaction == null) return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();
        _transaction = null;
    }


    public void Dispose()
    {
        //if (DbContext == null)
        //    return;
        ////
        //// Close connection
        //if (DbContext.Database.GetDbConnection().State == ConnectionState.Open)
        //{
        //    DbContext.Database.GetDbConnection().Close();
        //}
        //DbContext.Dispose();

        //DbContext = null;
    }

    private async Task StartNewTransactionIfNeeded()
    {
        if (_transaction == null)
            _transaction = _isolationLevel.HasValue
                ? await DbContext.Database.BeginTransactionAsync(_isolationLevel.GetValueOrDefault())
                : await DbContext.Database.BeginTransactionAsync();
    }
}