using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    public class Item
    {
        [Key]
        public int ItemID;
        public int LocationID;
        public string Name;
        public string Description;
        public string Link;

        [ForeignKey("LocationID")]
        public virtual Location Location { set; get; }
    }
}
