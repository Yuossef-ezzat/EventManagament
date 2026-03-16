using DomainLayer.Models.EventModule;


namespace DomainLayer.Models.PaymentModule
{
    public class Payment : BaseEntity<int>
    {
        public string UserId { get; set; }
        public int EventId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public ApplicationUser User { get; set; }

        public Event Event { get; set; }
    }
}
