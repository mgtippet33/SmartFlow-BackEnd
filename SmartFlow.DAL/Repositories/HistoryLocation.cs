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
        private SmartFlowContext dataBase;

        public HistoryLocationRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public HistoryLocation Get(int id)
        {
            return dataBase.historyLocations
                .Include(history => history.Location)
                .Include(history => history.Visitor)
                .SingleOrDefault(history => history.HistoryLocationID == id);
        }

        public IEnumerable<HistoryLocation> GetAll()
        {
            return dataBase.historyLocations
                .Include(history => history.Location)
                .Include(history => history.Visitor)
                .ToList();
        }

        public int Create(HistoryLocation history)
        {
            history.Visitor = dataBase.visitors
                .Find(history.Visitor.VisitorID);
            history.Location = dataBase.locations
                .Find(history.Location.LocationID);
            dataBase.historyLocations.Add(history);
            dataBase.SaveChanges();

            return history.HistoryLocationID;
        }

        public void Delete(int id)
        {
            HistoryLocation history = Get(id);
            if (history != null)
            {
                dataBase.historyLocations.Remove(history);
            }
        }

        public void Update(HistoryLocation history)
        {
            var toUpdateHistory = dataBase.historyLocations.FirstOrDefault(
                eventHistory =>
                eventHistory.HistoryLocationID == history.HistoryLocationID);
            if (toUpdateHistory != null)
            {
                toUpdateHistory.HistoryLocationID = history.HistoryLocationID;
                toUpdateHistory.Visitor = dataBase.visitors
                    .Find(history.Visitor.VisitorID);
                toUpdateHistory.Location = dataBase.locations
                    .Find(history.Location.LocationID);
                toUpdateHistory.Action = history.Action ?? toUpdateHistory.Action;
                toUpdateHistory.ActionTime = history.ActionTime;
            }
        }
    }
}
