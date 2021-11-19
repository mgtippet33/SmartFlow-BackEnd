using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.EF
{
    public class SmartFlowContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        internal DbSet<User> users { get; set; }
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(user => user.UserID)
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
