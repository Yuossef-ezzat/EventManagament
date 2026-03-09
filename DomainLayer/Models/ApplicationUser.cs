using DomainLayer.Models.EventModule;
using DomainLayer.Models.PaymentModule;
using DomainLayer.Models.Registeration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<Event> OrganizedEvents { get; set; }
        public List<Registration> Registrations { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
