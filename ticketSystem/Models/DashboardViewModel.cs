using System.Collections.Generic;

namespace ticketSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public Dictionary<string, int> TicketsByStatus { get; set; } = new();

        // ← swap out Ticket for DashboardTicketDto
        public List<DashboardTicketDto> FilteredTickets { get; set; } = new();

        public int? SelectedProjectId { get; set; }
        public int? SelectedCategoryId { get; set; }
        public int? SelectedPriorityId { get; set; }
        public string? SelectedStatus { get; set; }
        public string? SelectedUserId { get; set; }
    }
}



public class DashboardTicketDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? CategoryName { get; set; }
    public string? PriorityName { get; set; }
    public string? AssignedTo { get; set; }
}
