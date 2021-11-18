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
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public LocationService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                }
                ).CreateMapper();

            fromDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<LocationDTO> GetAllLocations()
        {
            var locations = database.Locations.GetAll();
            var locationsDTO = toDTOMapper.Map<IEnumerable<Location>,
                List<LocationDTO>>(locations);

            return locationsDTO;
        }

        public LocationDTO GetLocation(int id)
        {
            var location = database.Locations.Get(id);
            if (location == null)
                throw new NullReferenceException();
            var locationDTO = toDTOMapper
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
                throw new ArgumentException();

            var location = fromDTOMapper.Map<LocationDTO, Location>(locationDTO);
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

            location = fromDTOMapper.Map<LocationDTO, Location>(locationDTO);
            database.Locations.Update(location);
            database.Save();
        }
    }
}
