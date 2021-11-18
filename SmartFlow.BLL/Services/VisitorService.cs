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
    public class VisitorService : IVisitorService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public VisitorService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<Visitor, VisitorDTO>()
                .ReverseMap()).CreateMapper();
            fromDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<VisitorDTO, Visitor>()
                .ReverseMap()).CreateMapper();
        }

        public VisitorDTO GetVisitor(int id)
        {
            var visitor = database.Visitors.Get(id);
            if (visitor == null)
                throw new ArgumentNullException();
            var visitorDTO = toDTOMapper
                .Map<Visitor, VisitorDTO>(visitor);

            return visitorDTO;
        }

        public IEnumerable<VisitorDTO> GetAllVisitors()
        {
            var visitors = database.Visitors.GetAll();
            var visitorsDTO = toDTOMapper.Map<IEnumerable<Visitor>,
                List<VisitorDTO>>(visitors);

            return visitorsDTO;
        }

        public int AddVisitor(VisitorDTO visitorDTO)
        {
            throw new NotImplementedException();
            // TO DO Identity
        }

        public void DeleteVisitor(int id)
        {
            var visitor = database.Visitors.Get(id);
            if (visitor == null)
                throw new NullReferenceException();

            database.Visitors.Delete(id);
            database.Save();
        }

        public void UpdateVisitor(VisitorDTO visitorDTO)
        {
            var visitor = database.Visitors.Get(visitorDTO.VisitorID);
            if (visitor == null)
                throw new NullReferenceException();
            var visitorExsist = database.Visitors.GetAll()
                .Any(currentVisitor =>
                    currentVisitor.Email == visitorDTO.Email &&
                    currentVisitor.VisitorID == visitorDTO.VisitorID);
            if (visitorExsist)
                throw new NullReferenceException();

            visitor = fromDTOMapper.Map<VisitorDTO, Visitor>(visitorDTO);
            database.Visitors.Update(visitor);
            database.Save();
        }
    }
}
