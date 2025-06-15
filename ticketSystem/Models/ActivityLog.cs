using System;

namespace ticketSystem.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Action { get; set; } // e.g. "Status Changed", "Assigned to User", etc.
        public string PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }

        // Navigation
        public Ticket Ticket { get; set; }
    }
}
