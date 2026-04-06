using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.RegisrationDtos
{
    public class RegistrationDto
    {
        public int RegistrationId { get; set; }

        // بيانات اليوزر
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        // بيانات الحدث
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; }
        public bool PaymentRequired { get; set; }

        // بيانات التسجيل
        public string Status { get; set; }        // Confirmed, Pending, Canceled
        public string PaymentStatus { get; set; } // Paid, Pending, Failed
        public DateTime RegisteredAt { get; set; }
    }
}
