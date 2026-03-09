using DomainLayer.Models.EventModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.PaymentModule
{
    public class Payment : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public ApplicationUser User { get; set; }

        public Event Event { get; set; }
    }
}
