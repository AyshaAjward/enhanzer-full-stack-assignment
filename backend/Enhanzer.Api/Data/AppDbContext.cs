using Enhanzer.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>()
            .ToTable("Location_Details");

        modelBuilder.Entity<Location>()
            .HasKey(x => x.Id);
    }
}