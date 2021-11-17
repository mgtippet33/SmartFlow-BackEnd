using Microsoft.EntityFrameworkCore;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.EF
{
    public class SmartFlowContext: DbContext
    {
        internal DbSet<Administrator> administrators { get; set; }
        internal DbSet<Visitor> visitors { get; set; }
        internal DbSet<BusinessPartner> businessPartners { get; set; }
        internal DbSet<Event> events { get; set; }
        internal DbSet<Location> locations { get; set; }
        internal DbSet<Item> items { get; set; }
        internal DbSet<EventRating> eventRatings { get; set; }
        internal DbSet<HistoryLocation> historyLocations { get; set; }

        public SmartFlowContext(DbContextOptions<SmartFlowContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=smartflowdb;Username=postgres;Password=admin");
        }
    }
}
