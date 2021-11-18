using Microsoft.EntityFrameworkCore;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.EF
{
    public class SmartFlowContext : DbContext
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>()
                .Property(admin => admin.AdministratorID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<BusinessPartner>()
                .Property(partner => partner.BusinessPartnerID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Visitor>()
                .Property(visitor => visitor.VisitorID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Event>()
                .Property(currentEvent => currentEvent.EventID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Location>()
                .Property(location => location.LocationID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Item>()
                .Property(item => item.ItemID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<EventRating>()
                .Property(rating => rating.EventRatingID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<HistoryLocation>()
                .Property(history => history.HistoryLocationID)
                .ValueGeneratedOnAdd();
        }
    }
}
