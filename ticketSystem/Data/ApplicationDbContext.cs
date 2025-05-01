using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ticketSystem.Models; // 👈 Add this line to import your Ticket model

namespace ticketSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 👇 Register the Tickets table
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }

    }
}
