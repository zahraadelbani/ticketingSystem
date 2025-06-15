namespace ticketSystem.Models
{
    public class Priority
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Level { get; set; }    // optional: use for ordering if you like
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
