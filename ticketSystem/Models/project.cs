using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ticketSystem.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
