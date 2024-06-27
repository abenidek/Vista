using Microsoft.EntityFrameworkCore;
using Vista.Data.Models;

namespace Vista.Data.AppDbContext
{
    public class VistaDbContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikedVideo> LikedVideos { get; set; }
        public DbSet<DislikedVideo> DislikedVideos { get; set; }
        public DbSet<WatchedVideo> WatchedVideos { get; set; }
        public DbSet<UserFollower> UserFollowers { get; set; }
        public DbSet<SavedVideo> SavedVideos { get; set; }

        public VistaDbContext(DbContextOptions<VistaDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Video>(v =>
            {
                v.Property(e => e.VideoId).HasDefaultValueSql("uuid_generate_v4()");
                v.Property(e => e.UploadDate).HasDefaultValueSql("Now()");
                v.Property(e => e.Likes).HasDefaultValue("0");
                v.Property(e => e.Dislikes).HasDefaultValue("0");
                v.Property(e => e.Views).HasDefaultValue("0");
            });

            modelBuilder.Entity<User>(u =>
            {
                u.Property(e => e.UserId).HasDefaultValueSql("uuid_generate_v4()");
                u.HasIndex(e => e.UserName).IsUnique();
                u.HasIndex(e => e.Email).IsUnique();
                u.Property(e => e.FollowersCount).HasDefaultValue("0");
                u.Property(e => e.FollowingCount).HasDefaultValue("0");
            });

            modelBuilder.Entity<Comment>(c =>
            {
                c.Property(c => c.CreatedAt).HasDefaultValueSql("Now()");
                c.Property(i => i.Id).HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<Category>(c =>
            {
                c.HasData(
                new Category { CategoryId = 1, CategoryName = "Sports" },
                new Category { CategoryId = 2, CategoryName = "Photography" },
                new Category { CategoryId = 3, CategoryName = "Travel" },
                new Category { CategoryId = 4, CategoryName = "Cooking" },
                new Category { CategoryId = 5, CategoryName = "Movies" },
                new Category { CategoryId = 6, CategoryName = "Music" },
                new Category { CategoryId = 7, CategoryName = "Art" },
                new Category { CategoryId = 8, CategoryName = "Reading" },
                new Category { CategoryId = 9, CategoryName = "Writing" },
                new Category { CategoryId = 10, CategoryName = "Dancing" },
                new Category { CategoryId = 11, CategoryName = "Gaming" }
                );
            });

            modelBuilder.Entity<LikedVideo>().HasKey(l => new { l.UserId, l.VideoId });

            modelBuilder.Entity<DislikedVideo>().HasKey(d => new { d.UserId, d.VideoId });

            modelBuilder.Entity<WatchedVideo>(wv =>
            {
                wv.HasKey(wv => new { wv.UserId, wv.VideoId });
                wv.Property(wv => wv.ViewedAt).HasDefaultValueSql("Now()");
            });

            modelBuilder.Entity<UserFollower>(fu =>
            {
                fu.HasKey(f => new { f.FollowedUserId, f.FollowerUserId });

                fu.HasOne(f => f.FollowerUser)
                    .WithMany()
                    .HasForeignKey(f => f.FollowerUserId)
                    .OnDelete(DeleteBehavior.Restrict);    
                
                fu.HasOne(f => f.FollowedUser)
                    .WithMany(u => u.Followers)
                    .HasForeignKey(f => f.FollowedUserId);
            });

            modelBuilder.Entity<SavedVideo>().HasKey(sv => new { sv.UserId, sv.VideoId });
        }
    }
}