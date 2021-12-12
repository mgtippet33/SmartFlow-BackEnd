using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.DTO
{
    public class LocationDTO
    {
        public int LocationID { set; get; }
        public int EventID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string State { set; get; }
        public EventDTO Event { set; get; }
    }
}
