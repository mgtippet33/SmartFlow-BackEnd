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
        private SmartFlowContext database;

        public EventRatingRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public EventRating Get(int id)
        {
            return database.eventRatings
                .Include(rating => rating.Visitor)
                .Include(rating => rating.Event)
                .SingleOrDefault(rating => rating.EventRatingID == id);
        }

        public IEnumerable<EventRating> GetAll()
        {
            return database.eventRatings
                .Include(rating => rating.Visitor)
                .Include(rating => rating.Event)
                .ToList();
        }

        public int Create(EventRating rating)
        {
            rating.Visitor = database.users
                .Find(rating.Visitor.UserID);
            rating.Event = database.events
                .Find(rating.Event.EventID);
            database.eventRatings.Add(rating);
            database.SaveChanges();

            return rating.EventRatingID;
        }

        public void Delete(int id)
        {
            EventRating rating = Get(id);
            if (rating != null)
            {
                database.eventRatings.Remove(rating);
            }
        }

        public void Update(EventRating rating)
        {
            var toUpdateRating = database.eventRatings.FirstOrDefault(
                eventRating => 
                eventRating.EventRatingID == rating.EventRatingID);
            if (toUpdateRating != null)
            {
                toUpdateRating.EventRatingID = rating.EventRatingID;
                toUpdateRating.Visitor = database.users
                    .Find(rating.Visitor.UserID);
                toUpdateRating.Event = database.events
                    .Find(rating.Event.EventID);
                toUpdateRating.Score = rating.Score;
                toUpdateRating.ScoreDate = rating.ScoreDate;
            }
        }
    }
}
