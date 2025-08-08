using CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<Interaction> Interactions { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<DealStatus> DealStatuses { get; set; }
    public DbSet<InteractionType> InteractionTypes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DealStatus>().HasData(
            new DealStatus { Id = 1, Name = "New" },
            new DealStatus { Id = 2, Name = "InProgress" },
            new DealStatus { Id = 3, Name = "Completed" }
        );

        modelBuilder.Entity<InteractionType>().HasData(
            new InteractionType { Id = 1, Name = "Call" },
            new InteractionType { Id = 2, Name = "Email" },
            new InteractionType { Id = 3, Name = "Meeting" }
        );
    }
}