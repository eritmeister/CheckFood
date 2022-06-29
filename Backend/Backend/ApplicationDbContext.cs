using Microsoft.EntityFrameworkCore;
using WebApi1.ModelsDatabase;

namespace WebApi1;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Marketplace> Marketplaces { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Product");
        modelBuilder.Entity<Marketplace>().ToTable("Marketplace");
    }
    
}