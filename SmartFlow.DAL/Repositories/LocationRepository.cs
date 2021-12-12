using Microsoft.EntityFrameworkCore;
using SmartFlow.DAL.EF;
using SmartFlow.DAL.Entities;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Repositories
{
    public class LocationRepository : IRepository<Location>
    {
        private SmartFlowContext database;

        public LocationRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public Location Get(int id)
        {
            return database.locations
                .Include(location => location.Event)
                .SingleOrDefault(location => location.LocationID == id);
        }

        public IEnumerable<Location> GetAll()
        {
            return database.locations
                .Include(location => location.Event)
                .ToList();
        }

        public int Create(Location location)
        {
            location.Event = database.events
                .Find(location.Event.EventID);
            database.locations.Add(location);
            database.SaveChanges();

            return location.LocationID;
        }

        public void Delete(int id)
        {
            Location location = Get(id);
            if (location != null)
            {
                database.locations.Remove(location);
            }
        }

        public void Update(Location location)
        {
            var toUpdateLocation = database.locations.FirstOrDefault(
                loc => loc.LocationID == location.LocationID);
            if (toUpdateLocation != null)
            {
                toUpdateLocation.LocationID = location.LocationID;
                toUpdateLocation.Event = database.events.
                    Find(location.Event.EventID);
                toUpdateLocation.Name = location.Name ?? toUpdateLocation.Name;
                toUpdateLocation.Description = location.Description ?? toUpdateLocation.Description;
                toUpdateLocation.State = location.State ?? toUpdateLocation.State;
            }
        }
    }
}
