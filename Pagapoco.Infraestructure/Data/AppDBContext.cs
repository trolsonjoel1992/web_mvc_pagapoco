using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Pagapoco.Infrastructure.Data;

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

        // Herencia TPH: Publicación puede ser Vehicle, Bike o Part
        modelBuilder.Entity<Publication>()
            .HasDiscriminator<string>("Type")
            .HasValue<Vehicle>("Vehicle")
            .HasValue<Bike>("Bike")
            .HasValue<Part>("Part");

        // Relación: User tiene muchas publicaciones
        modelBuilder.Entity<Publication>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);

        // Relación: Image ➝ Publication
        modelBuilder.Entity<Image>()
            .HasOne(i => i.Publication)
            .WithMany(p => p.Images)
            .HasForeignKey(i => i.PublicationId);

        // Restricciones opcionales
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);
    }
}
