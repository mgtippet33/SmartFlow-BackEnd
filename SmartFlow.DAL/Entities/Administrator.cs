using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    public class Administrator
    {
        [Key]
        public int AdministratorID;
        public string Name;
        public string Email;
        public string Password;
    }
}
