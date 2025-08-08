using System.ComponentModel.DataAnnotations;

namespace CRM.Models;

public class Deal
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int StatusId { get; set; }
    public DealStatus Status { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int ManagerId { get; set; }
    public Manager Manager { get; set; }

    public int? ProductId { get; set; }
    public Product? Product { get; set; }
}

public class DealStatus
{
    public int Id { get; set; }
    public string Name { get; set; } // "New", "InProgress", "Completed"...

    public List<Deal> Deals { get; set; } = new();
}

public class CreateDealDto
{
    [Range(0.01, 1000000, ErrorMessage = "Сумма должна быть больше 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Укажите дату")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Укажите клиента")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Укажите статус")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Укажите менеджера")]
    public int ManagerId { get; set; }

    public int? ProductId { get; set; } = null;
}
