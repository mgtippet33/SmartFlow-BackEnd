using AutoMapper;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using SmartFlow.DAL.Entities;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Services
{
    public class UserService : IUserService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public UserService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<User, UserDTO>()
                .ReverseMap()).CreateMapper();
            fromDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<UserDTO, User>()
                .ReverseMap()).CreateMapper();
        }

        public UserDTO GetUser(int id)
        {
            var user = database.Users.Get(id);
            if (user == null)
                throw new ArgumentNullException();
            var userDTO = toDTOMapper
                .Map<User, UserDTO>(user);

            return userDTO;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = database.Users.GetAll();
            var usersDTO = toDTOMapper.Map<IEnumerable<User>,
                List<UserDTO>>(users);

            return usersDTO;
        }

        public int AddUser(UserDTO userDTO)
        {
            var user = fromDTOMapper.Map<UserDTO, User>(userDTO);
            var userID = database.Users.Create(user);
            return userID;
        }

        public void DeleteUser(int id)
        {
            var user = database.Users.Get(id);
            if (user == null)
                throw new NullReferenceException();

            database.Users.Delete(id);
            database.Save();
        }

        public void UpdateUser(UserDTO userDTO)
        {
            var user = database.Users
                .Get(userDTO.UserID);
            if (user == null)
                throw new NullReferenceException();
            var userExsist = database.Users.GetAll()
                .Any(usr => usr.Email == userDTO.Email &&
                    usr.UserID == userDTO.UserID);
            if (userExsist)
                throw new NullReferenceException();

            user = fromDTOMapper.Map<UserDTO, User>(userDTO);
            database.Users.Update(user);
            database.Save();
        }
    }
}
