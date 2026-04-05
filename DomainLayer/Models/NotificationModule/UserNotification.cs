namespace DomainLayer.Models.NotificationModule;

public class UserNotification : BaseEntity<int>
{
    public int UserId { get; set; }
    public int NotifId { get; set; }
    public bool IsRead { get; set; } = false;
    public ApplicationUser? User { get; set; }
    public Notification? Notification { get; set; }
}
