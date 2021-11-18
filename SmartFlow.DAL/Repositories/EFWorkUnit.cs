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
    public class EFWorkUnit : IWorkUnit
    {
        private SmartFlowContext database;
        private AdministratorRepository administratorRepository;
        private VisitorRepository visitorRepository;
        private BusinessPartnerRepository businessPartnerRepository;
        private EventRepository eventRepository;
        private LocationRepository locationRepository;
        private ItemRepository itemRepository;
        private EventRatingRepository eventRatingRepository;
        private HistoryLocationRepository historyLocationRepository;

        public EFWorkUnit(SmartFlowContext context)
        {
            database = context;
        }

        public IRepository<Administrator> Administrators
        {
            get
            {
                if (administratorRepository == null)
                {
                    administratorRepository = new AdministratorRepository(database);
                }
                return administratorRepository;
            }
        }

        public IRepository<Visitor> Visitors
        {
            get
            {
                if (visitorRepository == null)
                {
                    visitorRepository = new VisitorRepository(database);
                }
                return visitorRepository;
            }
        }

        public IRepository<BusinessPartner> BusinessPartners
        {
            get
            {
                if (businessPartnerRepository == null)
                {
                    businessPartnerRepository = new BusinessPartnerRepository(database);
                }
                return businessPartnerRepository;
            }
        }

        public IRepository<Event> Events
        {
            get
            {
                if (eventRepository == null)
                {
                    eventRepository = new EventRepository(database);
                }
                return eventRepository;
            }
        }

        public IRepository<Location> Locations
        {
            get
            {
                if (locationRepository == null)
                {
                    locationRepository = new LocationRepository(database);
                }
                return locationRepository;
            }
        }

        public IRepository<Item> Items
        {
            get
            {
                if (itemRepository == null)
                {
                    itemRepository = new ItemRepository(database);
                }
                return itemRepository;
            }
        }

        public IRepository<EventRating> EventRatings
        {
            get
            {
                if (eventRatingRepository == null)
                {
                    eventRatingRepository = new EventRatingRepository(database);
                }
                return eventRatingRepository;
            }
        }

        public IRepository<HistoryLocation> HistoryLocations
        {
            get
            {
                if (historyLocationRepository == null)
                {
                    historyLocationRepository = new HistoryLocationRepository(database);
                }
                return historyLocationRepository;
            }
        }

        public void Save()
        {
            database.SaveChanges();
        }
    }
}
