using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IEventRatingService
    {
        IEnumerable<EventRatingDTO> GetAllEventRatings();
        EventRatingDTO GetEventRating(int id);
        EventRatingDTO GetRatingByEvent(int eventID);
        int AddEventRating(EventRatingDTO eventRatingDTO);
        void DeleteEventRating(int id);
        void UpdateEventRating(EventRatingDTO eventRatingDTO);
    }
}
