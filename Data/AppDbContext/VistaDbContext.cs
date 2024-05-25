using Microsoft.EntityFrameworkCore;
using Vista.Data.Models;

namespace Vista.Data.AppDbContext
{
    public class VistaDbContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User> Categorys { get; set; }
        public DbSet<User> Comments { get; set; }
        public DbSet<User> LikesAndDislikes { get; set; }
        public DbSet<User> WatchedVideos { get; set; }

        public VistaDbContext(DbContextOptions<VistaDbContext> options) : base(options)
        {

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
            });

            modelBuilder.Entity<Comment>(c =>
            {
                c.Property(c => c.CreatedAt).HasDefaultValueSql("Now()");
                c.Property(i => i.Id).HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<Category>(c => c.Property(i => i.CategoryId).HasDefaultValueSql("uuid_generate_v4()"));

            modelBuilder.Entity<LikeAndDislike>().HasKey(ld => new { ld.UserId, ld.VideoId });

            modelBuilder.Entity<WatchedVideo>(wv =>
            {
                wv.HasKey(wv => new { wv.UserId, wv.VideoId });
                wv.Property(wv => wv.ViewedAt).HasDefaultValueSql("Now()");
            });
        }
    }
}