using DomainLayer.Models.NotificationModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace PresistenceLayer.Data.Configurations
{

     public class UserNotificationConfig : IEntityTypeConfiguration<UserNotification>
     {
         public void Configure(EntityTypeBuilder<UserNotification> builder)
         {
            //builder.HasKey(un => new { un.UserId, un.NotifId });

            builder.HasOne(un => un.Notification)
                   .WithMany(n => n.UserNotifications)
                   .HasForeignKey(n => n.NotifId);

            builder.HasOne(un => un.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(n => n.UserId);
        }
     }
}
