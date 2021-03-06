using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("historyLocation")]
    public class HistoryLocation
    {
        [Key]
        public int HistoryLocationID { set; get; }
        public int LocationID;
        public int VisitorID;
        public bool Came { set; get; }
        public bool CameOut { set; get; }
        public DateTime ActionTime { set; get; }

        [ForeignKey("LocationID")]
        public virtual Location Location { set; get; }

        [ForeignKey("VisitorID")]
        public virtual User Visitor { set; get; }
    }
}
