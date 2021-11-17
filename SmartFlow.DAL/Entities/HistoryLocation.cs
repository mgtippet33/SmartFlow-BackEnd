using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    public class HistoryLocation
    {
        [Key]
        public int HistoryLocationID;
        public int LocationID;
        public int VisitorID;
        public string Action;
        public DateTime ActionTime;

        [ForeignKey("LocationID")]
        public virtual Location Location { set; get; }

        [ForeignKey("VisitorID")]
        public virtual Visitor Visitor { set; get; }
    }
}
