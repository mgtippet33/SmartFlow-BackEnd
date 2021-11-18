using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Entities
{
    [Table("administrator")]
    public class Administrator
    {
        [Key]
        public int AdministratorID { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
