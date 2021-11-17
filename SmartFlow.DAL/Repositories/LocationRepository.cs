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
        private SmartFlowContext dataBase;

        public LocationRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public Location Get(int id)
        {
            return dataBase.locations
                .Include(location => location.Event)
                .SingleOrDefault(location => location.LocationID == id);
        }

        public IEnumerable<Location> GetAll()
        {
            return dataBase.locations
                .Include(location => location.Event)
                .ToList();
        }

        public int Create(Location location)
        {
            location.Event = dataBase.events
                .Find(location.Event.EventID);
            dataBase.locations.Add(location);
            dataBase.SaveChanges();

            return location.LocationID;
        }

        public void Delete(int id)
        {
            Location location = Get(id);
            if (location != null)
            {
                dataBase.locations.Remove(location);
            }
        }

        public void Update(Location location)
        {
            var toUpdateLocation = dataBase.locations.FirstOrDefault(
                loc => loc.EventID == location.EventID);
            if (toUpdateLocation != null)
            {
                toUpdateLocation.LocationID = location.LocationID;
                toUpdateLocation.Event = dataBase.events.
                    Find(location.Event.EventID);
                toUpdateLocation.Name = location.Name ?? toUpdateLocation.Name;
                toUpdateLocation.Description = location.Description ?? toUpdateLocation.Description;
                toUpdateLocation.State = location.State ?? toUpdateLocation.State;
            }
        }
    }
}
