using ArtworkSharing.Core.Interfaces.Repositories;
using ArtworkSharing.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ArtworkSharing.DAL.Repositories;

namespace ArtworkSharing.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DbContext { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public IArtworkRepository ArtworkRepository { get; private set; }

        public IArtistPackageRepository ArtworkPackageRepository { get; private set; }

        public IArtistRepository ArtistRepository { get; private set; }

        public IArtworkServiceRepository ArtworkServiceRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public IFollowRepository FollowRepository { get; private set; }

        public ILikeRepository LikeRepository { get; private set; }

        public IMediaContentRepository MediaContentRepository { get; private set; }

        public IPackageRepository PackageRepository { get; private set; }

        public IRatingRepository RatingRepository { get; private set; }

        public IRefundRequestRepository RefundRequestRepository { get; private set; }

        public ITransactionRepository TransactionRepository { get; private set; }

        public IVNPayTransactionRepository VNPayTransactionRepository { get; private set; }

        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;

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
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue ?
                    await DbContext.Database.BeginTransactionAsync(_isolationLevel.GetValueOrDefault()) : await DbContext.Database.BeginTransactionAsync();
            }
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
            if (DbContext == null)
                return;
            //
            // Close connection
            if (DbContext.Database.GetDbConnection().State == ConnectionState.Open)
            {
                DbContext.Database.GetDbConnection().Close();
            }
            DbContext.Dispose();

            DbContext = null;
        }


    }
}
