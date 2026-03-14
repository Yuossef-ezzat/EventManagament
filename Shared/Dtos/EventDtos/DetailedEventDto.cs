using DomainLayer.Models.EventModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.EventDtos
{
    public class DetailedEventDto
    {
        public int Id { get; set; }
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
