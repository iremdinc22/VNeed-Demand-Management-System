using Microsoft.EntityFrameworkCore;
using Vneed.Data.Models;

namespace Vneed.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Demand> Demand { get; set; }
    public DbSet<UserToken> UserToken { get; set; }
    public DbSet<DemandStatusHistory> DemandStatusHistories { get; set; }
    public DbSet<ProductSuggestion> ProductSuggestions { get; set; }
    public DbSet<UserMailToken> UserMailTokens { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //  Global query filters for soft delete
        modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Role>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Product>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Category>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<Demand>().HasQueryFilter(e => e.DeletedAt == null);
        modelBuilder.Entity<ProductSuggestion>().HasQueryFilter(e => e.DeletedAt == null);
        
        // Tablo isimleri
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<Category>().ToTable("category");
        modelBuilder.Entity<Product>().ToTable("product");
        modelBuilder.Entity<Demand>().ToTable("demand");
        modelBuilder.Entity<UserToken>().ToTable("user_token");
        modelBuilder.Entity<DemandStatusHistory>().ToTable("demand_status_history");
        modelBuilder.Entity<ProductSuggestion>().ToTable("product_suggestion");
        modelBuilder.Entity<UserMailToken>().ToTable("user_mail_token");

        // İlişkiler
        modelBuilder.Entity<User>()
            .HasMany(u => u.Demands)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Demands)
            .WithOne(d => d.Product)
            .HasForeignKey(d => d.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        
           modelBuilder.Entity<User>()
               .HasMany(u => u.UserTokens)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
           .HasMany(u => u.ProductSuggestions)
           .WithOne(ps => ps.User)
           .HasForeignKey(ps => ps.UserId)
           .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Category>()
            .HasMany(c => c.ProductSuggestions)
            .WithOne(ps => ps.Category)
            .HasForeignKey(ps => ps.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Demand>()
            .HasMany(d => d.StatusHistories)   
            .WithOne(h => h.Demand)
            .HasForeignKey(h => h.DemandId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Demand>()
            .Property(d => d.Status)
            .HasConversion<string>();

        modelBuilder.Entity<DemandStatusHistory>()
            .Property(h => h.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserMailTokens)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "User" },
            new Role { Id = 2, Name = "TeamLead" },
            new Role { Id = 3, Name = "Admin" }
        );
    }
}