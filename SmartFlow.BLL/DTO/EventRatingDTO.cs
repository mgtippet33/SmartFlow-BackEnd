﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.DTO
{
    public class EventRatingDTO
    {
        public int EventRatingID { set; get; }
        public double Score { set; get; }
        public DateTime ScoreDate { set; get; }
        public virtual VisitorDTO Visitor { set; get; }
        public virtual EventDTO Event { set; get; }
    }
}
