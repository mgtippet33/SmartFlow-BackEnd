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
    public class EventRepository : IRepository<Event>
    {
        private SmartFlowContext database;

        public EventRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public Event Get(int id)
        {
            return database.events
                .Include(currentEvent => currentEvent.BusinessPartner)
                .SingleOrDefault(currentEvent => currentEvent.EventID == id);
        }

        public IEnumerable<Event> GetAll()
        {
            return database.events
                .Include(currentEvent => currentEvent.BusinessPartner)
                .ToList();
        }

        public int Create(Event currentEvent)
        {
            currentEvent.BusinessPartner = database.users
                .Find(currentEvent.BusinessPartner.UserID);
            database.events.Add(currentEvent);
            database.SaveChanges();

            return currentEvent.EventID;
        }

        public void Delete(int id)
        {
            Event currentEvent = Get(id);
            if (currentEvent != null)
            {
                database.events.Remove(currentEvent);
            }
        }

        public void Update(Event currrentEvent)
        {
            var toUpdateEvent = database.events.FirstOrDefault(
                ev => ev.EventID == currrentEvent.EventID);
            if (toUpdateEvent != null)
            {
                toUpdateEvent.EventID = currrentEvent.EventID;
                toUpdateEvent.BusinessPartner = database.users.
                    Find(currrentEvent.BusinessPartner.UserID);
                toUpdateEvent.Name = currrentEvent.Name ?? toUpdateEvent.Name;
                toUpdateEvent.Description = currrentEvent.Description ?? toUpdateEvent.Description;
                toUpdateEvent.Coordinates = currrentEvent.Coordinates ?? toUpdateEvent.Coordinates;
                toUpdateEvent.OpenTime = currrentEvent.OpenTime;
                toUpdateEvent.CloseTime = currrentEvent.CloseTime;
            }
        }
    }
}
