using CRM.Data;
using CRM.Models;

public class SeedDataService
{
    public static async Task Initialize(IServiceProvider serviceProvider, bool force = false)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();

        // Статусы сделок
        if (force || !db.DealStatuses.Any())
        {
            db.DealStatuses.RemoveRange(db.DealStatuses); // Очистка при force
            db.DealStatuses.AddRange(
                new DealStatus { Name = "New" },
                new DealStatus { Name = "InProgress" },
                new DealStatus { Name = "Completed" }
            );
        }

        // Типы взаимодействий
        if (force || !db.InteractionTypes.Any())
        {
            db.InteractionTypes.RemoveRange(db.InteractionTypes);
            db.InteractionTypes.AddRange(
                new InteractionType { Name = "Call" },
                new InteractionType { Name = "Email" },
                new InteractionType { Name = "Meeting" }
            );
        }

        // Менеджеры
        if (force || !db.Managers.Any())
        {
            db.Managers.AddRange(
                new Manager { Name = "Алексей Петров", Email = "alex@example.com" },
                new Manager { Name = "Мария Иванова", Email = "maria@example.com" }
            );
        }

        // Продукты
        if (force || !db.Products.Any())
        {
            db.Products.AddRange(
                new Product { Name = "Базовый пакет", Description = "Стартовое решение", Price = 50000 },
                new Product { Name = "Профессиональный пакет", Description = "Расширенный функционал", Price = 150000 }
            );
        }

        await db.SaveChangesAsync();
    }
}