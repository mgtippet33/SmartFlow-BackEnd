using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class VisitorModel
    {
        public int VisitorID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
