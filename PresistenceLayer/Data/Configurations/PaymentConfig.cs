using DomainLayer.Models.PaymentModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer.Data.Configurations
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                 


            builder.HasOne(p => p.Event)
                .WithMany(e => e.Payments)
                .HasForeignKey(p => p.EventId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
