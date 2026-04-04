using DomainLayer.Models.EventModule;
using System.ComponentModel.DataAnnotations;

namespace DomainLayer.Models.NotificationModule;

public class Notification : BaseEntity<int>
{
    [Key]
    public int NotifId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<UserNotification>? UserNotifications { get; set; }
}

