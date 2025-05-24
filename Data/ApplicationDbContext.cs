using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<ContactMessage> ContactMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<ContactMessage>()
            .HasIndex(c => c.Email);

        // Add default owner user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1, // Using negative ID for seed data
                Username = "Owner",
                Email = "owner@example.com",
                Password = "none", // adding default password
                Rank = UserRank.Owner
            }
        );
    }
}