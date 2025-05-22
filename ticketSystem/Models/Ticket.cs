using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? AssignedTo { get; set; }  // This will store the username or UserId

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy { get; set; }

        [Required(ErrorMessage = "Please select a project")]
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project? Project { get; set; }
    }
}
