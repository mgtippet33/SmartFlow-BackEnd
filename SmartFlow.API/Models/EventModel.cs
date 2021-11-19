using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class EventModel
    {
        public int EventID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Coordinates { set; get; }
        public TimeSpan OpenTime { set; get; }
        public TimeSpan CloseTime { set; get; }
        public BusinessPartnerModel BusinessPartner { set; get; }
    }
}
