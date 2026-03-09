using DomainLayer.Models;
using DomainLayer.Models.EventModule;
using DomainLayer.Models.PaymentModule;
using DomainLayer.Models.Registeration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer
{
    public class EventDbContext: IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
    {
        public EventDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Registration> Registerations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);
            base.OnModelCreating(builder);
        }

    }
}
