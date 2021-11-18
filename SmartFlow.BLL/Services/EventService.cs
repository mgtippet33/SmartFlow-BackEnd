using AutoMapper;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using SmartFlow.DAL.Entities;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Services
{
    public class EventService : IEventService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public EventService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<BusinessPartner, BusinessPartnerDTO>().ReverseMap();
                }
                ).CreateMapper();

            fromDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                    cfg.CreateMap<BusinessPartnerDTO, BusinessPartner>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<EventDTO> GetAllEvents()
        {
            var events = database.Events.GetAll();
            var eventsDTO = toDTOMapper.Map<IEnumerable<Event>,
                List<EventDTO>>(events);

            return eventsDTO;
        }

        public EventDTO GetEvent(int id)
        {
            var currentEvent = database.Events.Get(id);
            if (currentEvent == null)
                throw new NullReferenceException();
            var eventDTO = toDTOMapper
                .Map<Event, EventDTO>(currentEvent);

            return eventDTO;
        }

        public int AddEvent(EventDTO eventDTO)
        {
            if (eventDTO.BusinessPartner == null)
                throw new ArgumentNullException();
            var eventExsist = database.Events.GetAll()
                .Any(ev => ev.Name == eventDTO.Name &&
                    ev.BusinessPartner.BusinessPartnerID ==
                    eventDTO.BusinessPartner.BusinessPartnerID);
            if (eventExsist)
                throw new ArgumentException();

            var currentEvent = fromDTOMapper.Map<EventDTO, Event>(eventDTO);
            var currentEventID = database.Events.Create(currentEvent);
            return currentEventID;
        }

        public void DeleteEvent(int id)
        {
            var currentEvent = database.Events.Get(id);
            if (currentEvent == null)
                throw new NullReferenceException();

            database.Events.Delete(id);
            database.Save();
        }

        public void UpdateEvent(EventDTO eventDTO)
        {
            var currentEvent = database.Events.Get(eventDTO.EventID);
            if (currentEvent == null)
                throw new NullReferenceException();
            var eventExsist = database.Events.GetAll()
                .Any(ev =>
                    ev.Name == eventDTO.Name &&
                    ev.BusinessPartner.BusinessPartnerID ==
                    eventDTO.BusinessPartner.BusinessPartnerID);
            if (eventExsist)
                throw new NullReferenceException();

            currentEvent = fromDTOMapper.Map<EventDTO, Event>(eventDTO);
            database.Events.Update(currentEvent);
            database.Save();
        }
    }
}
