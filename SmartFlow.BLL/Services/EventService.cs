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
        private IMapper mapper;
        private IWorkUnit database;

        public EventService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<EventDTO> GetAllEvents(int userID)
        {
            var events = database.Events.GetAll()
                .OrderBy(ev => ev.EventID).ToList();

            var userRole = database.Users.Get(userID).Role;
            if (userRole != "Administrator")
            {
                events.RemoveAll(ev => ev.BusinessPartnerID != userID);
            }
            var eventsDTO = mapper.Map<IEnumerable<Event>,
                List<EventDTO>>(events);

            return eventsDTO;
        }

        public EventDTO GetEvent(int id)
        {
            var currentEvent = database.Events.Get(id);
            if (currentEvent == null)
                throw new NullReferenceException("This event does not exist.");
            var eventDTO = mapper
                .Map<Event, EventDTO>(currentEvent);

            return eventDTO;
        }

        public int AddEvent(EventDTO eventDTO)
        {
            if (eventDTO.BusinessPartner == null)
                throw new ArgumentNullException();
            var eventExsist = database.Events.GetAll()
                .Any(ev => ev.Name == eventDTO.Name &&
                    ev.BusinessPartner.UserID ==
                    eventDTO.BusinessPartner.UserID);
            if (eventExsist)
                throw new ArgumentException("An event with this name already exists.");

            var currentEvent = mapper.Map<EventDTO, Event>(eventDTO);
            var currentEventID = database.Events.Create(currentEvent);
            return currentEventID;
        }

        public void DeleteEvent(int id)
        {
            var currentEvent = database.Events.Get(id);
            if (currentEvent == null)
                throw new NullReferenceException("This event does not exist.");

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
                    ev.EventID != eventDTO.EventID &&
                    ev.Name == eventDTO.Name &&
                    ev.BusinessPartner.UserID ==
                    eventDTO.BusinessPartner.UserID);
            if (eventExsist)
                throw new ArgumentException("An event with this name already exists.");
            currentEvent = mapper.Map<EventDTO, Event>(eventDTO);
            database.Events.Update(currentEvent);
            database.Save();
        }
    }
}
