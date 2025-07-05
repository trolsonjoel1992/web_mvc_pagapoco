using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;

namespace Pagapoco.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Publication> Publications => Set<Publication>();
        public DbSet<Image> Images => Set<Image>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación: User tiene muchas publicaciones
            modelBuilder.Entity<Publication>()
                .HasOne<User>()
                .WithMany(u => u.Publications)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Image ➝ Publication
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Publication)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Restricción: Email único en User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Valor por defecto para IsDeleted en User
            modelBuilder.Entity<User>()
                .Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            // Valor por defecto para IsPaused en Publication
            modelBuilder.Entity<Publication>()
                .Property(p => p.IsPaused)
                .HasDefaultValue(false);
        }
    }
}