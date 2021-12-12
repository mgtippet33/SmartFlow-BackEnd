using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.DTO
{
    public class ItemDTO
    {
        public int ItemID { set; get; }
        public int LocationID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Link { set; get; }
        public LocationDTO Location { set; get; }
    }
}
