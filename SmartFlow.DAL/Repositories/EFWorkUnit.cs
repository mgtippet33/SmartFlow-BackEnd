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
        private SmartFlowContext dataBase;
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
            dataBase = context;
        }

        public IRepository<Administrator> Administrators
        {
            get
            {
                if (administratorRepository != null)
                {
                    administratorRepository = new AdministratorRepository(dataBase);
                }
                return administratorRepository;
            }
        }

        public IRepository<Visitor> Visitors
        {
            get
            {
                if (visitorRepository != null)
                {
                    visitorRepository = new VisitorRepository(dataBase);
                }
                return visitorRepository;
            }
        }

        public IRepository<BusinessPartner> BusinessPartners
        {
            get
            {
                if (businessPartnerRepository != null)
                {
                    businessPartnerRepository = new BusinessPartnerRepository(dataBase);
                }
                return businessPartnerRepository;
            }
        }

        public IRepository<Event> Events
        {
            get
            {
                if (eventRepository != null)
                {
                    eventRepository = new EventRepository(dataBase);
                }
                return eventRepository;
            }
        }

        public IRepository<Location> Locations
        {
            get
            {
                if (locationRepository != null)
                {
                    locationRepository = new LocationRepository(dataBase);
                }
                return locationRepository;
            }
        }

        public IRepository<Item> Items
        {
            get
            {
                if (itemRepository != null)
                {
                    itemRepository = new ItemRepository(dataBase);
                }
                return itemRepository;
            }
        }

        public IRepository<EventRating> EventRatings
        {
            get
            {
                if (eventRatingRepository != null)
                {
                    eventRatingRepository = new EventRatingRepository(dataBase);
                }
                return eventRatingRepository;
            }
        }

        public IRepository<HistoryLocation> HistoryLocations
        {
            get
            {
                if (historyLocationRepository != null)
                {
                    historyLocationRepository = new HistoryLocationRepository(dataBase);
                }
                return historyLocationRepository;
            }
        }

        public void Save()
        {
            dataBase.SaveChanges();
        }
    }
}
