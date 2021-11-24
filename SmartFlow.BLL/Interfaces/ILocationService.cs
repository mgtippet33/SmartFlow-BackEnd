using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface ILocationService
    {
        IEnumerable<LocationDTO> GetAllLocations();
        IEnumerable<LocationDTO> GetLocationsByEvent(int eventID);
        LocationDTO GetLocation(int id);
        int AddLocation(LocationDTO locationDTO);
        void DeleteLocation(int id);
        void UpdateLocation(LocationDTO locationDTO);
    }
}
