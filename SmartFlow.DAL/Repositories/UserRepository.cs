using SmartFlow.DAL.EF;
using SmartFlow.DAL.Entities;
using SmartFlow.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private SmartFlowContext database;

        public UserRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public User Get(int id)
        {
            return database.users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return database.users.ToList();
        }

        public int Create(User user)
        {
            database.users.Add(user);
            database.SaveChanges();

            return user.UserID;
        }

        public void Delete(int id)
        {
            User user = Get(id);
            if (user != null)
            {
                database.users.Remove(user);
            }
        }

        public void Update(User user)
        {
            var toUpdateUser = database.users.FirstOrDefault(
                user => user.UserID == user.UserID);
            if (toUpdateUser != null)
            {
                toUpdateUser.UserID = user.UserID;
                toUpdateUser.Name = user.Name ?? toUpdateUser.Name;
                toUpdateUser.Email = user.Email ?? toUpdateUser.Email;
                toUpdateUser.PasswordHash = user.PasswordHash ?? toUpdateUser.PasswordHash;
            }
        }
    }
}
