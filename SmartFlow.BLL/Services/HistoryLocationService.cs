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
    public class HistoryLocationService : IHistoryLocationService
    {
        private IMapper mapper;
        private IWorkUnit database;

        public HistoryLocationService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<HistoryLocation, HistoryLocationDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                    cfg.CreateMap<HistoryLocationDTO, HistoryLocation>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<HistoryLocationDTO> GetAllHistoryLocations()
        {
            var histories = database.HistoryLocations.GetAll()
                .OrderBy(history => history.HistoryLocationID);
            var historiesDTO = mapper.Map<IEnumerable<HistoryLocation>,
                List<HistoryLocationDTO>>(histories);

            return historiesDTO;
        }

        public HistoryLocationDTO GetHistoryLocation(int id)
        {
            var history = database.HistoryLocations.Get(id);
            if (history == null)
                throw new NullReferenceException();
            var historyDTO = mapper
                .Map<HistoryLocation, HistoryLocationDTO>(history);

            return historyDTO;
        }

        public int AddHistoryLocation(HistoryLocationDTO historyDTO)
        {
            if (historyDTO.Location == null || historyDTO.Visitor == null)
                throw new ArgumentNullException();
            var historyExsist = database.HistoryLocations.GetAll()
                .Any(hist => hist.Location.LocationID ==
                    historyDTO.Location.LocationID &&
                    hist.Visitor.UserID == historyDTO.Visitor.UserID &&
                    hist.Came == historyDTO.Came &&
                    hist.ActionTime.Date == historyDTO.ActionTime.Date);
            if (historyExsist)
                throw new ArgumentException();

            var history = mapper.Map<HistoryLocationDTO, HistoryLocation>(historyDTO);
            var historyID = database.HistoryLocations.Create(history);
            return historyID;
        }

        public void DeleteHistoryLocation(int id)
        {
            var history = database.HistoryLocations.Get(id);
            if (history == null)
                throw new NullReferenceException();

            database.HistoryLocations.Delete(id);
            database.Save();
        }

        public void UpdateHistoryLocation(HistoryLocationDTO historyDTO)
        {
            var history = database.HistoryLocations
                .GetAll()
                .Where(history => history.VisitorID == historyDTO.Visitor.UserID &&
                        history.Came == true && history.CameOut == false &&
                        history.ActionTime.Date == historyDTO.ActionTime.Date)
                .FirstOrDefault();
            //var history = database.HistoryLocations.Get(historyDTO.HistoryLocationID);
            if (history == null)
                throw new NullReferenceException();
            var historyExsist = database.HistoryLocations.GetAll()
                .Any(hist => hist.Location.LocationID ==
                    historyDTO.Location.LocationID &&
                    hist.Visitor.UserID == historyDTO.Visitor.UserID &&
                    hist.CameOut == historyDTO.CameOut &&
                    hist.ActionTime == historyDTO.ActionTime);
            if (historyExsist)
                throw new NullReferenceException();

            historyDTO.HistoryLocationID = history.HistoryLocationID;
            history = mapper.Map<HistoryLocationDTO, HistoryLocation>(historyDTO);
            database.HistoryLocations.Update(history);
            database.Save();
        }
    }
}
