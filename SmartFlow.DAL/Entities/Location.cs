using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("location")]
    public class Location
    {
        [Key]
        public int LocationID { set; get; }
        public int EventID;
        public string Name { set; get; }
        public string Description { set; get; }
        public string State { set; get; }

        [ForeignKey("EventID")]
        public virtual Event Event { set; get; }
    }
}
