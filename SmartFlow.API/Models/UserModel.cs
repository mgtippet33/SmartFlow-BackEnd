using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.API.Models
{
    public class UserModel
    {
        public int UserID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string Role { set; get; }
    }
}
