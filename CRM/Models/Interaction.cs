namespace CRM.Models;

public class Interaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int TypeId { get; set; }
    public InteractionType Type { get; set; }  

    public string Notes { get; set; }

    // Связь с клиентом
    public int ClientId { get; set; }
    public Client Client { get; set; }
}

public class InteractionType
{
    public int Id { get; set; }
    public string Name { get; set; } // "Call", "Email", "Meeting"...

    public List<Interaction> Interactions { get; set; } = new();
}
