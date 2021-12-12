using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Interfaces
{
    public interface IWorkUnit
    {
        IRepository<User> Users { get; }
        IRepository<Event> Events { get; }
        IRepository<Location> Locations { get; }
        IRepository<Item> Items { get; }
        IRepository<EventRating> EventRatings { get; }
        IRepository<HistoryLocation> HistoryLocations { get; }
        void Save();
    }
}
