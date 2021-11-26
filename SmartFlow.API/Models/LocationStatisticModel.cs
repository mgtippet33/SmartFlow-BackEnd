using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class LocationStatisticModel
    {
        public int LocationID { set; get; }
        public string LocationName { set; get; }
        public int Visits { set; get; }
        public DateTime DateVisits { set; get; }
    }
}
