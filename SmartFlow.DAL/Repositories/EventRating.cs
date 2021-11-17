using Microsoft.EntityFrameworkCore;
using SmartFlow.DAL.EF;
using SmartFlow.DAL.Entities;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Repositories
{
    public class EventRatingRepository : IRepository<EventRating>
    {
        private SmartFlowContext dataBase;

        public EventRatingRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public EventRating Get(int id)
        {
            return dataBase.eventRatings
                .Include(rating => rating.Visitor)
                .Include(rating => rating.Event)
                .SingleOrDefault(rating => rating.EventRatingID == id);
        }

        public IEnumerable<EventRating> GetAll()
        {
            return dataBase.eventRatings
                .Include(rating => rating.Visitor)
                .Include(rating => rating.Event)
                .ToList();
        }

        public int Create(EventRating rating)
        {
            rating.Visitor = dataBase.visitors
                .Find(rating.Visitor.VisitorID);
            rating.Event = dataBase.events
                .Find(rating.Event.EventID);
            dataBase.eventRatings.Add(rating);
            dataBase.SaveChanges();

            return rating.EventRatingID;
        }

        public void Delete(int id)
        {
            EventRating rating = Get(id);
            if (rating != null)
            {
                dataBase.eventRatings.Remove(rating);
            }
        }

        public void Update(EventRating rating)
        {
            var toUpdateRating = dataBase.eventRatings.FirstOrDefault(
                eventRating => 
                eventRating.EventRatingID == rating.EventRatingID);
            if (toUpdateRating != null)
            {
                toUpdateRating.EventRatingID = rating.EventRatingID;
                toUpdateRating.Visitor = dataBase.visitors
                    .Find(rating.Visitor.VisitorID);
                toUpdateRating.Event = dataBase.events
                    .Find(rating.Event.EventID);
                toUpdateRating.Score = rating.Score;
                toUpdateRating.ScoreDate = rating.ScoreDate;
            }
        }
    }
}
