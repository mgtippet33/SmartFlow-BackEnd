using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    public class Event
    {
        [Key]
        public int EventID;
        public int BusinessPartnerID;
        public string Name;
        public string Description;
        public string Coordinates;
        public TimeSpan OpenTime;
        public TimeSpan CloseTime;

        [ForeignKey("BusinessPartnerID")]
        public virtual BusinessPartner BusinessPartner { set; get; }

    }
}
