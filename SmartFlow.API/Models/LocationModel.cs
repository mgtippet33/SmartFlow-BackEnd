using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class LocationModel
    {
        public int LocationID { set; get; }
        public int EventID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string State { set; get; }
        public EventModel Event { set; get; }
    }
}
