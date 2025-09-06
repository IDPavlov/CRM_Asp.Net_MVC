using Microsoft.EntityFrameworkCore;
using CRM.Data;

namespace CRM;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Конфигурация сервисов
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<CrmDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Обработка аргументов командной строки
        if (args.Contains("--seed"))
        {
            Console.WriteLine("Seeding database...");
            using (var scope = app.Services.CreateScope())
            {
                await SeedDataService.Initialize(scope.ServiceProvider, force: true);
            }
            Console.WriteLine("Seeding complete. Exiting.");
            return; // Завершаем работу после заполнения
        }

        // Стандартный pipeline для работы приложения
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapStaticAssets();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

        app.MapControllerRoute(
            name: "clients",
            pattern: "{controller=Clients}/{action=Index}/{id?}").WithStaticAssets();

        app.MapControllerRoute(
            name: "clients",
            pattern: "{controller=Clients}/{action=Create}").WithStaticAssets();

        app.MapControllerRoute(
            name: "clients",
            pattern: "{controller=Clients}/{action=Details}").WithStaticAssets();

        app.MapControllerRoute(
            name: "clients",
            pattern: "{controller=Clients}/{action=Delete}").WithStaticAssets();

        app.MapControllerRoute(
            name: "products",
            pattern: "{controller=DealProducts}/{action=Index}/{id?}").WithStaticAssets();

        app.MapControllerRoute(
            name: "products",
            pattern: "{controller=DealProducts}/{action=Create}").WithStaticAssets();

        app.MapControllerRoute(
            name: "deals",
            pattern: "{controller=Deals}/{action=Index}/{id?}").WithStaticAssets();

        app.MapControllerRoute(
            name: "deals",
            pattern: "{controller=Deals}/{action=Create}").WithStaticAssets();

        app.MapControllerRoute(
            name: "deals",
            pattern: "{controller=Deals}/{action=Delete}").WithStaticAssets();

        await app.RunAsync();
    }
}