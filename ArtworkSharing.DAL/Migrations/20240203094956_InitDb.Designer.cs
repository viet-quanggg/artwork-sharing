﻿// <auto-generated />
using System;
using ArtworkSharing.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArtworkSharing.DAL.Migrations
{
    [DbContext(typeof(ArtworkSharingContext))]
    [Migration("20240203094956_InitDb")]
    partial class InitDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtistPackage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PurchasedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("PackageId");

                    b.HasIndex("TransactionId");

                    b.ToTable("ArtistPackages");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Artwork", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Artworks");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtworkCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ArtworkCategories");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtworkService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AudienceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RequestedDeadlineDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("RequestedDeposit")
                        .HasColumnType("real");

                    b.Property<float>("RequestedPrice")
                        .HasColumnType("real");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("AudienceId");

                    b.ToTable("ArtworkServices");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CommentedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CommentedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("CommentedUserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Follow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Follows");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LikedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("LikedUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("LikedUserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.MediaContent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Capacity")
                        .HasColumnType("real");

                    b.Property<string>("Media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("ArtworkServiceId");

                    b.ToTable("MediaContents");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Package", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Rating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Star")
                        .HasColumnType("real");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.RefundRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefundRequestDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("RefundRequests");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ArtworkServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AudienceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<float>("TotalBill")
                        .HasColumnType("real");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArtworkId");

                    b.HasIndex("ArtworkServiceId");

                    b.HasIndex("AudienceId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("IsArtistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IsArtistId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtistPackage", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artist", "Artist")
                        .WithMany("ArtistPackages")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Package");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Artwork", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artist", "Artist")
                        .WithMany("Artworks")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtworkCategory", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artwork", "Artwork")
                        .WithMany("Categories")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artwork");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtworkService", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artist", "Artist")
                        .WithMany("ArtworkServices")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "Audience")
                        .WithMany("ArtworkServices")
                        .HasForeignKey("AudienceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Audience");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Comment", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artwork", "Artwork")
                        .WithMany("Comments")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "CommentedUser")
                        .WithMany("Comments")
                        .HasForeignKey("CommentedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Artwork");

                    b.Navigation("CommentedUser");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Follow", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "Followed")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "Follower")
                        .WithMany("Followings")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Like", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artwork", "Artwork")
                        .WithMany("Likes")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "LikedUser")
                        .WithMany("Likes")
                        .HasForeignKey("LikedUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Artwork");

                    b.Navigation("LikedUser");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.MediaContent", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artwork", "Artwork")
                        .WithMany("MediaContents")
                        .HasForeignKey("ArtworkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.ArtworkService", null)
                        .WithMany("ArtworkProduct")
                        .HasForeignKey("ArtworkServiceId");

                    b.Navigation("Artwork");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Rating", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.RefundRequest", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artwork", null)
                        .WithMany("Transactions")
                        .HasForeignKey("ArtworkId");

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.ArtworkService", null)
                        .WithMany("Transactions")
                        .HasForeignKey("ArtworkServiceId");

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.User", "Audience")
                        .WithMany("Transactions")
                        .HasForeignKey("AudienceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Audience");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.User", b =>
                {
                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Artist", "IsArtist")
                        .WithMany()
                        .HasForeignKey("IsArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArtworkSharing.Core.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IsArtist");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Artist", b =>
                {
                    b.Navigation("ArtistPackages");

                    b.Navigation("ArtworkServices");

                    b.Navigation("Artworks");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.Artwork", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("MediaContents");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.ArtworkService", b =>
                {
                    b.Navigation("ArtworkProduct");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("ArtworkSharing.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("ArtworkServices");

                    b.Navigation("Comments");

                    b.Navigation("Followers");

                    b.Navigation("Followings");

                    b.Navigation("Likes");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
