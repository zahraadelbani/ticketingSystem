using Microsoft.EntityFrameworkCore.Migrations;

namespace ticketSystem.Data.Migrations
{
    public partial class UpdateUsernames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update usernames to be the same as email (without the @domain part)
            migrationBuilder.Sql(@"
                UPDATE AspNetUsers 
                SET UserName = SUBSTRING(Email, 1, CHARINDEX('@', Email) - 1)
                WHERE UserName IS NULL OR UserName = Email;
            ");

            // Update tickets to use the new usernames
            migrationBuilder.Sql(@"
                UPDATE Tickets 
                SET CreatedBy = SUBSTRING(CreatedBy, 1, CHARINDEX('@', CreatedBy) - 1)
                WHERE CreatedBy LIKE '%@%';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No down migration needed as this is a one-way update
        }
    }
} 