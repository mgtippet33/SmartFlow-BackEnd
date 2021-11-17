using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    public class Location
    {
        [Key]
        public int LocationID;
        public int EventID;
        public string Name;
        public string Description;
        public string State;

        [ForeignKey("EventID")]
        public virtual Event Event { set; get; }
    }
}
