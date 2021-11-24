using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class ItemModel
    {
        public int ItemID { set; get; }
        public int LocationID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Link { set; get; }
        public LocationModel Location { set; get; }
    }
}
