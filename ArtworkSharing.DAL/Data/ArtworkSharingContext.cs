using ArtworkSharing.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtworkSharing.DAL.Data
{
    public class ArtworkSharingContext : DbContext
    {
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        public ArtworkSharingContext()
        {
            
        }
        public ArtworkSharingContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
               .HasForeignKey(l => l.LikedUserId)
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
}
