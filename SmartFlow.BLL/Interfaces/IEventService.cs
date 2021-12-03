using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IEventService
    {
        IEnumerable<EventDTO> GetAllEvents(int userID);
        EventDTO GetEvent(int id);
        int AddEvent(EventDTO eventDTO);
        void DeleteEvent(int id);
        void UpdateEvent(EventDTO eventDTO);
    }
}
