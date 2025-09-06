using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class DealProduct
    {
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int DealId { get; set; }
        public Deal Deal { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class CreateDealProductDto
    {
        [Range(1, 10000, ErrorMessage = "Количество должно быть больше 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Укажите продукт")]
        public int ProductId { get; set; }
    }
}
