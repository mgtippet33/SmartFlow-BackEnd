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
    public class EventRatingService : IEventRatingService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public EventRatingService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventRating, EventRatingDTO>().ReverseMap();
                    cfg.CreateMap<Visitor, VisitorDTO>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                }
                ).CreateMapper();

            fromDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventRatingDTO, EventRating>().ReverseMap();
                    cfg.CreateMap<VisitorDTO, Visitor>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<EventRatingDTO> GetAllEventRatings()
        {
            var ratings = database.EventRatings.GetAll();
            var ratingsDTO = toDTOMapper.Map<IEnumerable<EventRating>,
                List<EventRatingDTO>>(ratings);

            return ratingsDTO;
        }

        public EventRatingDTO GetEventRating(int id)
        {
            var rating = database.EventRatings.Get(id);
            if (rating == null)
                throw new NullReferenceException();
            var ratingDTO = toDTOMapper
                .Map<EventRating, EventRatingDTO>(rating);

            return ratingDTO;
        }

        public int AddEventRating(EventRatingDTO ratingDTO)
        {
            if (ratingDTO.Event == null || ratingDTO.Visitor == null)
                throw new ArgumentNullException();
            var ratingExsist = database.EventRatings.GetAll()
                .Any(rate => rate.Event.EventID == ratingDTO.Event.EventID &&
                    rate.Visitor.VisitorID == ratingDTO.Visitor.VisitorID);
            if (ratingExsist)
                throw new ArgumentException();

            var rating = fromDTOMapper.Map<EventRatingDTO, EventRating>(ratingDTO);
            var ratingID = database.EventRatings.Create(rating);
            return ratingID;
        }

        public void DeleteEventRating(int id)
        {
            var rating = database.EventRatings.Get(id);
            if (rating == null)
                throw new NullReferenceException();

            database.EventRatings.Delete(id);
            database.Save();
        }

        public void UpdateEventRating(EventRatingDTO ratingDTO)
        {
            var rating = database.EventRatings.Get(ratingDTO.EventRatingID);
            if (rating == null)
                throw new NullReferenceException();
            var ratingExsist = database.EventRatings.GetAll()
                .Any(rate => rate.Event.EventID == ratingDTO.Event.EventID &&
                    rate.Visitor.VisitorID == ratingDTO.Visitor.VisitorID);
            if (ratingExsist)
                throw new NullReferenceException();

            rating = fromDTOMapper.Map<EventRatingDTO, EventRating>(ratingDTO);
            database.EventRatings.Update(rating);
            database.Save();
        }
    }
}
