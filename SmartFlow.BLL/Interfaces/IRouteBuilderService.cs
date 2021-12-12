using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IRouteBuilderService
    {
        public IEnumerable<LocationDTO> GetRouteForVisitorByEvent(int eventID);
    }
}
