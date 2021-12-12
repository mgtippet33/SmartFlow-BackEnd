using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.DTO
{
    public class HistoryLocationDTO
    {
        public int HistoryLocationID { set; get; }
        public int LocationID { set; get; }
        public int VisitorID { set; get; }
        public bool Came { set; get; }
        public bool CameOut { set; get; }
        public DateTime ActionTime { set; get; }
        public virtual LocationDTO Location { set; get; }
        public virtual UserDTO Visitor { set; get; }
    }
}
