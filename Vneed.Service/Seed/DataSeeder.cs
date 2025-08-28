using Vneed.Data.Models;
using Microsoft.EntityFrameworkCore;
using Vneed.Data.Context;
using Vneed.Data.Enum;
using Vneed.Common.Helpers.HashPassword;

namespace Vneed.Service.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Veritabanı migrate edilir (varsa eksik migration)
        await context.Database.MigrateAsync();

        // --- Role ---
        if (!context.Role.Any())
        {
            context.Role.AddRange(new List<Role>
            {
                new Role { Name = "User" },
                new Role { Name = "TeamLead" },
                new Role { Name = "Admin" }
            });
        }

        // --- User ---
        if (!context.User.Any())
        {
            context.User.AddRange(new List<User>
            {
                new User {
                    FullName = "Admin Kullanıcı",
                    Email = "cansu@valde.co",
                    PasswordHash = HashHelper.ComputeSha256Hash("123456"),
                    RoleId = 3,
                    CreatedAt = DateTime.UtcNow
                },
                new User {
                    FullName = "Irmak Arı",
                    Email = "irmak.ari@valde.co",
                    PasswordHash = HashHelper.ComputeSha256Hash("123456"),
                    RoleId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new User {
                    FullName = "İrem Dinç",
                    Email = "irem.dinc@valde.co",
                    PasswordHash = HashHelper.ComputeSha256Hash("123456"),
                    RoleId = 1,
                    CreatedAt = DateTime.UtcNow
                },
                new User {
                    FullName = "Ferhat Sezer",
                    Email = "ferhat@valde.co",
                    PasswordHash = HashHelper.ComputeSha256Hash("123456"),
                    RoleId = 2,
                    CreatedAt = DateTime.UtcNow
                }
            });
        }

        // --- Category ---
        if (!context.Category.Any())
        {
            context.Category.AddRange(new List<Category>
            {
                new Category { Name = "Elektronik", CreatedAt = DateTime.UtcNow },
                new Category { Name = "Ofis Malzemeleri",CreatedAt = DateTime.UtcNow },
                new Category { Name = "Yiyecek", CreatedAt = DateTime.UtcNow },
                new Category { Name = "İçecek", CreatedAt = DateTime.UtcNow }
            });
        }

        // --- Product ---
        if (!context.Product.Any())
        {
            context.Product.AddRange(new List<Product>
            {
                new Product { Name = "Dizüstü Bilgisayar", CategoryId = 17, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Yazıcı", CategoryId = 17, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Post-it Not", CategoryId = 18, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Popkek", CategoryId = 19, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Maden Suyu", CategoryId = 20, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Kahve", CategoryId = 20, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Sandviç", CategoryId = 19, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Monitör", CategoryId = 17, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Product { Name = "Zımba", CategoryId = 18, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            });
        }

        // --- Product Suggestion ---
        if (!context.ProductSuggestions.Any())
        {
            context.ProductSuggestions.AddRange(new List<ProductSuggestion>
            {
                new ProductSuggestion { SuggestedName = "Masa Lambası", UserId = 2, CategoryId = 18, CreatedAt = DateTime.UtcNow },
                new ProductSuggestion { SuggestedName = "Adobe Lisansı", UserId = 2, CategoryId = 19, CreatedAt = DateTime.UtcNow }
            });
        }

        // --- Demand ---
        if (!context.Demand.Any())
        {
            context.Demand.AddRange(new List<Demand>
            {
                new Demand {
                    UserId = 2, ProductId = 6, Status = DemandStatus.PendingTeamLeadApproval,
                    Note = "Yeni laptop talebi", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 7, Status = DemandStatus.Approved,
                    Note = "Ofise yazıcı alınmalı", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 8, Status = DemandStatus.Rejected,
                    Note = "Post-it alınması gerekliydi ama uygun görülmedi.",
                    CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 6, Status = DemandStatus.Completed,
                    Note = "Laptop siparişi verildi.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 9, Status = DemandStatus.PendingTeamLeadApproval,
                    Note = "Atıştırmalık popkek talebi", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 10, Status = DemandStatus.PendingTeamLeadApproval,
                    Note = "Toplantı için maden suyu siparişi", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 11, Status = DemandStatus.Approved,
                    Note = "Ofis için kahve stokları yenilenmeli", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 12, Status = DemandStatus.Rejected,
                    Note = "Sandviç alımı uygun görülmedi.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 13, Status = DemandStatus.Completed,
                    Note = "Yeni personel için monitör siparişi verildi.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                },
                new Demand {
                    UserId = 2, ProductId = 14, Status = DemandStatus.PendingTeamLeadApproval,
                    Note = "Zımba eksikliği nedeniyle yeni talep oluşturuldu.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                }
            });
        }

        // Veritabanına tüm değişiklikleri kaydet
        await context.SaveChangesAsync();
    }
}
