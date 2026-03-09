using DomainLayer.Models.Registeration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer.Data.Configurations
{
    public class RegistrationConfig : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasOne(r => r.User)
                   .WithMany(u => u.Registrations)
                   .HasForeignKey(r => r.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Event)
                   .WithMany(e => e.Registrations)
                   .HasForeignKey(r => r.EventId)
                     .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
