using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("event")]
    public class Event
    {
        [Key]
        public int EventID { set; get; }
        public int BusinessPartnerID;
        public string Name { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Coordinates { set; get; }
        public string OpenTime { set; get; }
        public string CloseTime { set; get; }

        [ForeignKey("BusinessPartnerID")]
        public virtual User BusinessPartner { set; get; }

    }
}
