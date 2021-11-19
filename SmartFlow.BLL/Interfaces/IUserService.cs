using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUser(int id);
        int AddUser(UserDTO visitorDTO);
        void DeleteUser(int id);
        void UpdateUser(UserDTO visitorDTO);
    }
}
