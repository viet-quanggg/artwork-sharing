using ArtworkSharing.Core.Domain.Entities;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Data;

public class ArtworkSharingContext : IdentityDbContext<User, Role,
    Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ArtworkSharingContext()
    {
    }

    public ArtworkSharingContext(DbContextOptions options) : base(options)

    {
    }


    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<ArtistPackage> ArtistPackages { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtworkService> ArtworkServices { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<MediaContent> MediaContents { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<RefundRequest> RefundRequests { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<VNPayTransaction> VNPayTransactions { get; set; }
    public DbSet<VNPayTransactionRefund> VNPayTransactionRefunds { get; set; }
    public DbSet<VNPayTransactionTransfer> VNPayTransactionTransfers { get; set; }
    public DbSet<PaymentEvent> PaymentEvents { get; set; }
    public DbSet<PaypalAmount> PaypalAmounts { get; set; }
    public DbSet<PaypalItem> PaypalItems { get; set; }
    public DbSet<PaypalOrder> PaypalOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
            options.UseSqlServer(
                "server =(local); database = ArtworkSharing;uid=sa;pwd=123456@Aa; TrustServerCertificate=True");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserRoles)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .IsRequired();

        modelBuilder.Entity<Role>()
            .HasMany(r => r.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(r => r.RoleId)
            .IsRequired();

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followed)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Followings)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ArtworkService>()
            .HasOne(artwork => artwork.Audience)
            .WithMany(audience => audience.ArtworkServices)
            .HasForeignKey(artwork => artwork.AudienceId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Like>()
            .HasOne(l => l.LikedUser)
            .WithMany(a => a.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.CommentedUser)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.CommentedUserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ArtistPackage>()
            .HasOne(ap => ap.Artist)
            .WithMany(a => a.ArtistPackages)
            .HasForeignKey(a => a.ArtistId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}