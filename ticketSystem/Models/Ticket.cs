using System;
using System.ComponentModel.DataAnnotations;

namespace ticketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        // 🔗 Foreign Key to Project
        [Required(ErrorMessage = "Please select a project")]
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; } = null!;
    }
}
