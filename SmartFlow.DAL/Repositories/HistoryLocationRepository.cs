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
    public class HistoryLocationRepository : IRepository<HistoryLocation>
    {
        private SmartFlowContext database;

        public HistoryLocationRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public HistoryLocation Get(int id)
        {
            return database.historyLocations
                .Include(history => history.Location)
                .Include(history => history.Visitor)
                .SingleOrDefault(history => history.HistoryLocationID == id);
        }

        public IEnumerable<HistoryLocation> GetAll()
        {
            return database.historyLocations
                .Include(history => history.Location)
                .Include(history => history.Visitor)
                .ToList();
        }

        public int Create(HistoryLocation history)
        {
            history.Visitor = database.users
                .Find(history.Visitor.UserID);
            history.Location = database.locations
                .Find(history.Location.LocationID);
            database.historyLocations.Add(history);
            database.SaveChanges();

            return history.HistoryLocationID;
        }

        public void Delete(int id)
        {
            HistoryLocation history = Get(id);
            if (history != null)
            {
                database.historyLocations.Remove(history);
            }
        }

        public void Update(HistoryLocation history)
        {
            var toUpdateHistory = database.historyLocations.FirstOrDefault(
                eventHistory =>
                eventHistory.HistoryLocationID == history.HistoryLocationID);
            if (toUpdateHistory != null)
            {
                toUpdateHistory.HistoryLocationID = history.HistoryLocationID;
                toUpdateHistory.Visitor = database.users
                    .Find(history.Visitor.UserID);
                toUpdateHistory.Location = database.locations
                    .Find(history.Location.LocationID);
                toUpdateHistory.Action = history.Action ?? toUpdateHistory.Action;
                toUpdateHistory.ActionTime = history.ActionTime;
            }
        }
    }
}
