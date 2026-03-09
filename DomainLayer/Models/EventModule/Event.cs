using DomainLayer.Models.PaymentModule;
using DomainLayer.Models.Registeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.EventModule
{
    public class Event : BaseEntity<int>
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
        public ApplicationUser Organizer { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Registration> Registrations { get; set; }

        public List<Payment> Payments { get; set; }

        public List<Notification> Notifications { get; set; }
        #endregion

    }
}
