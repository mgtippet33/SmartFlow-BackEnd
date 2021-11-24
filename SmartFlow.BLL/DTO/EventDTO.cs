using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.DTO
{
    public class EventDTO
    {
        public int EventID { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Coordinates { set; get; }
        public string OpenTime { set; get; }
        public string CloseTime { set; get; }
        public UserDTO BusinessPartner { set; get; }
    }
}
