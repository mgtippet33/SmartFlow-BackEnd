using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class EventRatingModel
    {
        public int EventRatingID { set; get; }
        public double Score { set; get; }
        public DateTime ScoreDate { set; get; }
        public virtual VisitorModel Visitor { set; get; }
        public virtual EventModel Event { set; get; }
    }
}
