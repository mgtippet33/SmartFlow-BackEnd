using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("item")]
    public class Item
    {
        [Key]
        public int ItemID { set; get; }
        public int LocationID;
        public string Name { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Link { set; get; }

        [ForeignKey("LocationID")]
        public virtual Location Location { set; get; }
    }
}
