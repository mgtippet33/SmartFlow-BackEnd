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
    public class LocationService : ILocationService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public LocationService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<LocationDTO> GetAllLocations()
        {
            var locations = database.Locations.GetAll()
                .OrderBy(location => location.LocationID);
            var locationsDTO = mapper.Map<IEnumerable<Location>,
                List<LocationDTO>>(locations);

            return locationsDTO;
        }

        public IEnumerable<LocationDTO> GetLocationsByEvent(int eventID)
        {
            var locations = database.Locations.GetAll();
            locations = locations.Where(location =>
                location.Event.EventID == eventID)
                .OrderBy(location => location.LocationID)
                .ToList();
            var locationsDTO = mapper.Map<IEnumerable<Location>,
                List<LocationDTO>>(locations);

            return locationsDTO;
        }

        public LocationDTO GetLocation(int id)
        {
            var location = database.Locations.Get(id);
            if (location == null)
                throw new NullReferenceException();
            var locationDTO = mapper
                .Map<Location, LocationDTO>(location);

            return locationDTO;
        }

        public int AddLocation(LocationDTO locationDTO)
        {
            if (locationDTO.Event == null)
                throw new ArgumentNullException();
            var locationExsist = database.Locations.GetAll()
                .Any(loc => loc.Name == locationDTO.Name &&
                    loc.Event.EventID == locationDTO.Event.EventID);
            if (locationExsist)
                throw new ArgumentException("This location already exists at this event.");

            var location = mapper.Map<LocationDTO, Location>(locationDTO);
            var locationID = database.Locations.Create(location);
            return locationID;
        }

        public void DeleteLocation(int id)
        {
            var location = database.Locations.Get(id);
            if (location == null)
                throw new NullReferenceException();

            database.Locations.Delete(id);
            database.Save();
        }

        public void UpdateLocation(LocationDTO locationDTO)
        {
            var location = database.Locations.Get(locationDTO.LocationID);
            if (location == null)
                throw new NullReferenceException();
            var locationExsist = database.Locations.GetAll()
                .Any(loc =>
                    loc.Name == locationDTO.Name &&
                    loc.Event.EventID == locationDTO.Event.EventID);
            if (locationExsist)
                throw new NullReferenceException();

            location = mapper.Map<LocationDTO, Location>(locationDTO);
            database.Locations.Update(location);
            database.Save();
        }
    }
}
