using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IStatisticsService
    {
        IEnumerable<EventStatisticDTO> GetEventTop(int userID);
        IEnumerable<LocationStatisticDTO> GetLocationStatisticsByEvent(int eventID);
    }
}
