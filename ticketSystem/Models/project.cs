using System;
using System.Collections.Generic;

namespace ticketSystem.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Link to Tickets (one-to-many)
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
