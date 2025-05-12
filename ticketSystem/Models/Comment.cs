using System;
using System.ComponentModel.DataAnnotations;

namespace ticketSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Optional: reference the Ticket and User
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
