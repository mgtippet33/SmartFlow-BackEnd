using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("eventRating")]
    public class EventRating
    {
        [Key]
        public int EventRatingID { set; get; }
        public int VisitorID;
        public int EventID;
        public double Score { set; get; }
        public DateTime ScoreDate { set; get; }

        [ForeignKey("VisitorID")]
        public virtual Visitor Visitor { set; get; }


        [ForeignKey("EventID")]
        public virtual Event Event { set; get; }
    }
}
