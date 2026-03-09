using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace PresistenceLayer.Data.Configurations
{

     public class NotificationConfig : IEntityTypeConfiguration<Notification>
     {
         public void Configure(EntityTypeBuilder<Notification> builder)
         {
             builder.HasKey(n => n.Id);

             builder.Property(n => n.Message)
                 .IsRequired()
                 .HasMaxLength(500);

             builder.HasOne(n => n.User)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Event)
                 .WithMany(e => e.Notifications)
                 .HasForeignKey(n => n.EventId)
                 .OnDelete(DeleteBehavior.NoAction);
        }
     }
}
