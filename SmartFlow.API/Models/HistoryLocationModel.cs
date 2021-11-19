﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class HistoryLocationModel
    {
        public int HistoryLocationID { set; get; }
        public int LocationID { set; get; }
        public int VisitorID { set; get; }
        public string Action { set; get; }
        public DateTime ActionTime { set; get; }
        public virtual LocationModel Location { set; get; }
        public virtual VisitorModel Visitor { set; get; }
    }
}
