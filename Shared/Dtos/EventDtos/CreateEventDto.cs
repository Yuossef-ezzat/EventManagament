using DomainLayer.Models.EventModule;


namespace Shared.Dtos.EventDtos
{
    public class CreateEventDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = null!;
        public int MaxAttendance { get; set; }
        public EventStatus EventStatus { get; set; }
        public bool PaymentRequired { get; set; }

        #region Relations
        public int OrganizerId { get; set; }
        public string OrganizerName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public List<string> Registrations { get; set; } // we need replace string with a more specific DTO if needed

        public List<string> Payments { get; set; }

        public List<string> Notifications { get; set; }
        #endregion
    }
}
