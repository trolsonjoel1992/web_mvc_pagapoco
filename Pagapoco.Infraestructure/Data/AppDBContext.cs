using Microsoft.EntityFrameworkCore;
using Pagapoco.Core.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Pagapoco.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Notification> Notifications => Set<Notification>();

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

        // Relación: Question ➝ User
        modelBuilder.Entity<Question>()
            .HasOne(q => q.User)
            .WithMany()
            .HasForeignKey(q => q.UserId);

        // Relación: Question ➝ Publication
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Publication)
            .WithMany(p => p.Questions)
            .HasForeignKey(q => q.PublicationId);

        // Relación: Answer ➝ Question
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId);

        // Relación: Notification ➝ User
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId);

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
