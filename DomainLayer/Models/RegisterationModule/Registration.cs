using DomainLayer.Models.EventModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Registeration
{
    public class Registration : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public RegistrationStatus RegisterationStatus { get; set; }
        public ApplicationUser User { get; set; }

        public Event Event { get; set; }

    }
}
