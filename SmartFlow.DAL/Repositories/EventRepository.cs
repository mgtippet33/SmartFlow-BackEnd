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
        private SmartFlowContext dataBase;

        public EventRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public Event Get(int id)
        {
            return dataBase.events
                .Include(currentEvent => currentEvent.BusinessPartner)
                .SingleOrDefault(currentEvent => currentEvent.EventID == id);
        }

        public IEnumerable<Event> GetAll()
        {
            return dataBase.events
                .Include(currentEvent => currentEvent.BusinessPartner)
                .ToList();
        }

        public int Create(Event currentEvent)
        {
            currentEvent.BusinessPartner = dataBase.businessPartners
                .Find(currentEvent.BusinessPartner.BusinessPartnerID);
            dataBase.events.Add(currentEvent);
            dataBase.SaveChanges();

            return currentEvent.EventID;
        }

        public void Delete(int id)
        {
            Event currentEvent = Get(id);
            if (currentEvent != null)
            {
                dataBase.events.Remove(currentEvent);
            }
        }

        public void Update(Event currrentEvent)
        {
            var toUpdateEvent = dataBase.events.FirstOrDefault(
                ev => ev.EventID == currrentEvent.EventID);
            if (toUpdateEvent != null)
            {
                toUpdateEvent.EventID = currrentEvent.EventID;
                toUpdateEvent.BusinessPartner = dataBase.businessPartners.
                    Find(currrentEvent.BusinessPartner.BusinessPartnerID);
                toUpdateEvent.Name = currrentEvent.Name ?? toUpdateEvent.Name;
                toUpdateEvent.Description = currrentEvent.Description ?? toUpdateEvent.Description;
                toUpdateEvent.Coordinates = currrentEvent.Coordinates ?? toUpdateEvent.Coordinates;
                toUpdateEvent.OpenTime = currrentEvent.OpenTime;
                toUpdateEvent.CloseTime = currrentEvent.CloseTime;
            }
        }
    }
}
