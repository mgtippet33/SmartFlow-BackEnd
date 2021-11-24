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
        private IMapper mapper;
        private IWorkUnit database;

        public EventRatingService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventRating, EventRatingDTO>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<EventRatingDTO, EventRating>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<EventRatingDTO> GetAllEventRatings()
        {
            var ratings = database.EventRatings.GetAll()
                .OrderBy(rating => rating.EventRatingID);
            var ratingsDTO = mapper.Map<IEnumerable<EventRating>,
                List<EventRatingDTO>>(ratings);

            return ratingsDTO;
        }

        public EventRatingDTO GetRatingByEvent(int eventID)
        {
            var ratings = database.EventRatings.GetAll();
            var averageRating = ratings.Where(rating =>
                rating.Event.EventID == eventID).Average(rating => rating.Score);

            var currentEvent = database.Events.Get(eventID);
            var ratingDTO = new EventRatingDTO()
            {
                Event = mapper.Map<Event, EventDTO>(currentEvent),
                Score = averageRating
            };

            return ratingDTO;
        }

        public EventRatingDTO GetEventRating(int id)
        {
            var rating = database.EventRatings.Get(id);
            if (rating == null)
                throw new NullReferenceException();
            var ratingDTO = mapper
                .Map<EventRating, EventRatingDTO>(rating);

            return ratingDTO;
        }

        public int AddEventRating(EventRatingDTO ratingDTO)
        {
            if (ratingDTO.Event == null || ratingDTO.Visitor == null)
                throw new ArgumentNullException();
            var ratingExsist = database.EventRatings.GetAll()
                .Any(rate => rate.Event.EventID == ratingDTO.Event.EventID &&
                    rate.Visitor.UserID == ratingDTO.Visitor.UserID);
            if (ratingExsist)
                throw new ArgumentException();

            var rating = mapper.Map<EventRatingDTO, EventRating>(ratingDTO);
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

            rating = mapper.Map<EventRatingDTO, EventRating>(ratingDTO);
            database.EventRatings.Update(rating);
            database.Save();
        }
    }
}
