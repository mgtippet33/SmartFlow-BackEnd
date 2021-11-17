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
    public class AdministratorRepository : IRepository<Administrator>
    {
        private SmartFlowContext dataBase;

        public AdministratorRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public Administrator Get(int id)
        {
            return dataBase.administrators.Find(id);
        }

        public IEnumerable<Administrator> GetAll()
        {
            return dataBase.administrators.ToList();
        }

        public int Create(Administrator administrator)
        {
            dataBase.administrators.Add(administrator);
            dataBase.SaveChanges();

            return administrator.AdministratorID;
        }

        public void Delete(int id)
        {
            Administrator administrator = Get(id);
            if (administrator != null)
            {
                dataBase.administrators.Remove(administrator);
            }
        }

        public void Update(Administrator administrator)
        {
            var toUpdateAdmin = dataBase.administrators.FirstOrDefault(
                admin => admin.AdministratorID == administrator.AdministratorID);
            if (toUpdateAdmin != null)
            {
                toUpdateAdmin.AdministratorID = administrator.AdministratorID;
                toUpdateAdmin.Name = administrator.Name ?? toUpdateAdmin.Name;
                toUpdateAdmin.Email = administrator.Email ?? toUpdateAdmin.Email;
                toUpdateAdmin.Password = administrator.Password ?? toUpdateAdmin.Password;
            }
        }
    }
}
