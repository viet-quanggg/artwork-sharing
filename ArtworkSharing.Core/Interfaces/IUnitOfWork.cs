using ArtworkSharing.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworkSharing.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        IUserRepository UserRepository { get; }
        IArtworkRepository ArtworkRepository { get; }
        IArtistPackageRepository ArtworkPackageRepository { get; }
        IArtistRepository ArtistRepository { get; }
        IArtworkServiceRepository ArtworkServiceRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }   
        IFollowRepository FollowRepository { get; }
        ILikeRepository LikeRepository { get; }
        IMediaContentRepository MediaContentRepository { get; }
        IPackageRepository PackageRepository { get; }
        IRatingRepository RatingRepository { get; }
        IRefundRequestRepository RefundRequestRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        /// <summary>
        /// Saves changes to database, previously opening a transaction
        /// only when none exists. The transaction is opened with isolation
        /// level set in Unit of Work before calling this method.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransaction();

        /// <summary>
        /// Commits the current transaction (does nothing when none exists).
        /// </summary>
        Task CommitTransaction();

        /// <summary>
        /// Rolls back the current transaction (does nothing when none exists).
        /// </summary>
        Task RollbackTransaction();

    }
}
