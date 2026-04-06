using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresistenceLayer.Data
{
    public class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        public EventDbContext CreateDbContext(string[] args)
        {
            Env.Load();
            var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();

            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("EventDbContext"));

            return new EventDbContext(optionsBuilder.Options);
        }
    }
}
