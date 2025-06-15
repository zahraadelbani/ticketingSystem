using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticketSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        // simple assigned-to string
        public string? AssignedTo { get; set; }

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

        // Category relationship
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // Priority relationship
        public int? PriorityId { get; set; }
        [ForeignKey("PriorityId")]
        public virtual Priority? Priority { get; set; }
    }
}
