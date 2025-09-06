using System.ComponentModel.DataAnnotations;

namespace CRM.Models;

public class Deal
{
    public int Id { get; set; }

    public decimal? ServicePrice { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int StatusId { get; set; }
    public DealStatus Status { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }

    public int ManagerId { get; set; }
    public Manager Manager { get; set; }

    public List<DealProduct> DealProducts { get; set; } = new();

    public decimal? TotalAmount => 
        DealProducts.Sum(dp => dp.Quantity * dp.UnitPrice) + (ServicePrice ?? 0);
}


public class DealStatus
{
    public int Id { get; set; }

    public string Name { get; set; } // "New", "InProgress", "Completed"...

    public List<Deal> Deals { get; set; } = new();
}


public class CreateDealDto
{
    [Required]
    public string DealType { get; set; }  // "product" или "service"

    [Range(0.01, 1000000, ErrorMessage = "Стоимость услуги должна быть больше 0")]
    public decimal? ServicePrice { get; set; }

    [Required(ErrorMessage = "Укажите клиента")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Укажите статус")]
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Укажите менеджера")]
    public int ManagerId { get; set; }

    public List<CreateDealProductDto> DealProducts { get; set; } = new();
}
