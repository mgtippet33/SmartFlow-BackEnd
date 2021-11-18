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
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public HistoryLocationService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<HistoryLocation, HistoryLocationDTO>().ReverseMap();
                    cfg.CreateMap<Visitor, VisitorDTO>().ReverseMap();
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                }
                ).CreateMapper();

            fromDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<HistoryLocationDTO, HistoryLocation>().ReverseMap();
                    cfg.CreateMap<VisitorDTO, Visitor>().ReverseMap();
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<HistoryLocationDTO> GetAllHistoryLocations()
        {
            var histories = database.HistoryLocations.GetAll();
            var historiesDTO = toDTOMapper.Map<IEnumerable<HistoryLocation>,
                List<HistoryLocationDTO>>(histories);

            return historiesDTO;
        }

        public HistoryLocationDTO GetHistoryLocation(int id)
        {
            var history = database.HistoryLocations.Get(id);
            if (history == null)
                throw new NullReferenceException();
            var historyDTO = toDTOMapper
                .Map<HistoryLocation, HistoryLocationDTO>(history);

            return historyDTO;
        }

        public int AddHistoryLocation(HistoryLocationDTO historyDTO)
        {
            if (historyDTO.Location == null || historyDTO.Visitor == null)
                throw new ArgumentNullException();
            //var historyExsist = database.HistoryLocations.GetAll()
            //    .Any(hist => hist.Event.EventID == historyDTO.Event.EventID &&
            //        hist.Visitor.VisitorID == historyDTO.Visitor.VisitorID);
            //if (historyExsist)
            //    throw new ArgumentException();

            var history = fromDTOMapper.Map<HistoryLocationDTO, HistoryLocation>(historyDTO);
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
            var history = database.HistoryLocations.Get(historyDTO.HistoryLocationID);
            if (history == null)
                throw new NullReferenceException();
            var historyExsist = database.HistoryLocations.GetAll()
                .Any(hist => hist.Location.LocationID == historyDTO.Location.LocationID &&
                    hist.Visitor.VisitorID == historyDTO.Visitor.VisitorID);
            if (historyExsist)
                throw new NullReferenceException();

            history = fromDTOMapper.Map<HistoryLocationDTO, HistoryLocation>(historyDTO);
            database.HistoryLocations.Update(history);
            database.Save();
        }
    }
}
