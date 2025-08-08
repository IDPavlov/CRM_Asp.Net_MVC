namespace CRM.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    // Навигационное свойство к сделкам
    public List<Deal> Deals { get; set; } = new();
}
