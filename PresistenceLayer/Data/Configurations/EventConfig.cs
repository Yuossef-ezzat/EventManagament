using DomainLayer.Models.EventModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer.Data.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Location)
                   .HasMaxLength(300);

            builder.HasOne(e => e.Organizer)
                   .WithMany(u => u.OrganizedEvents)
                   .HasForeignKey(e => e.OrganizerId)
                     .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
