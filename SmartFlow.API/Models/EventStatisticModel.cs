using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class EventStatisticModel
    {
        public int EventID { set; get; }
        public string EventName { set; get; }
        public int AllVisits { set; get; }
    }
}
