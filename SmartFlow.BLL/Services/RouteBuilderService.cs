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
    public class RouteBuilderService : IRouteBuilderService
    {
        private IWorkUnit database;
        private IMapper mapper;

        public RouteBuilderService(IWorkUnit database)
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

        public IEnumerable<LocationDTO> GetRouteForVisitorByEvent(int eventID)
        {
            var histories = database.HistoryLocations.GetAll();
            var locationsByEvent = database.Locations
                .GetAll()
                .Where(location => location.EventID == eventID)
                .Select(location => location.LocationID)
                .ToList();
            var locationIDs = (from history in histories
                            where locationsByEvent.Contains(history.LocationID) &&
                                history.Came == true && history.CameOut == false &&
                                history.Location.State == "open" &&
                                history.ActionTime.Date == DateTime.Today.Date
                            group history by history.LocationID into hist
                            orderby hist.Count()
                            select hist.Key).ToList();
            var locations = database.Locations.GetAll()
                .Where(location => locationIDs.Contains(location.LocationID))
                .OrderBy(location =>
                    locationIDs.FindIndex(locationID => location.LocationID == locationID))
                .ToList();


            var locationsDTO = mapper.Map<List<Location>,
                List<LocationDTO>>(locations);
            return locationsDTO;
        }
    }
}
