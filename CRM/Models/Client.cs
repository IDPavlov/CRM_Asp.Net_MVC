using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return $"{Name} Email: {Email} Phone: {Phone}";
        }

        public List<Deal> Deals { get; set; } = new();
        public List<Interaction> Interactions { get; set; } = new();
    }
}
