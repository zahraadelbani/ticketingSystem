using Microsoft.EntityFrameworkCore.Migrations;

namespace ticketSystem.Data.Migrations
{
    public partial class UpdateTicketCreatedByToUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update CreatedBy in Tickets to use UserName from AspNetUsers where CreatedBy matches Email
            migrationBuilder.Sql(@"
                UPDATE t
                SET t.CreatedBy = u.UserName
                FROM Tickets t
                INNER JOIN AspNetUsers u ON t.CreatedBy = u.Email
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration needed
        }
    }
} 