using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IHistoryLocationService
    {
        IEnumerable<HistoryLocationDTO> GetAllHistoryLocations();
        HistoryLocationDTO GetHistoryLocation(int id);
        int AddHistoryLocation(HistoryLocationDTO historyLocationDTO);
        void DeleteHistoryLocation(int id);
        void UpdateHistoryLocation(HistoryLocationDTO historyLocationDTO);
    }
}
