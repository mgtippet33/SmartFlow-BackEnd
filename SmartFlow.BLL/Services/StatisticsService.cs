using AutoMapper;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Services
{
    public class StatisticsService : IStatisticsService
    {
        private IWorkUnit database;

        public StatisticsService(IWorkUnit database)
        {
            this.database = database;
        }

        public IEnumerable<EventStatisticDTO> GetEventTop(int userID)
        {
            var histories = database.HistoryLocations.GetAll();
            var locations = database.Locations.GetAll();
            var events = database.Events.GetAll();

            foreach (var location in locations)
            {
                location.Event = events
                    .Where(ev => ev.EventID == location.EventID)
                    .FirstOrDefault();
            }

            foreach (var history in histories)
            {
                history.Location.Event = locations
                    .Where(location => location.LocationID == history.LocationID)
                    .FirstOrDefault().Event;
            }

            histories = histories
                .Where(history => history.Location.Event.BusinessPartnerID == userID)
                .ToList();
            var statistics = from history in histories
                             where history.Came == true &&
                                history.CameOut == true
                             group history by (history.Location.Event.EventID,
                                    history.Location.Event.Name) into hist
                             orderby hist.Count() descending
                             select new EventStatisticDTO
                                 {
                                     EventID = hist.Key.EventID,
                                     EventName = hist.Key.Name,
                                     AllVisits = hist.Count()
                                  };

            return statistics;
        }

        public IEnumerable<LocationStatisticDTO> GetLocationStatisticsByEvent(int eventID)
        {
            var histories = database.HistoryLocations.GetAll();

            var statistics = from history in histories
                            where history.Location.EventID == eventID &&
                                history.Came == true &&
                                history.CameOut == true
                            group history by (history.Location.LocationID,
                                history.Location.Name,
                                history.ActionTime.Date) into hist
                            orderby hist.Key.Date
                            select new LocationStatisticDTO
                            {
                                LocationID = hist.Key.LocationID,
                                LocationName = hist.Key.Name,
                                DateVisits = hist.Key.Date,
                                Visits = hist.Count()
                            };

            return statistics;
        }
    }
}
