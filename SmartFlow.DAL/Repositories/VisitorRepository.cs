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
    public class VisitorRepository : IRepository<Visitor>
    {
        private SmartFlowContext dataBase;

        public VisitorRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public Visitor Get(int id)
        {
            return dataBase.visitors.Find(id);
        }

        public IEnumerable<Visitor> GetAll()
        {
            return dataBase.visitors.ToList();
        }

        public int Create(Visitor visitor)
        {
            dataBase.visitors.Add(visitor);
            dataBase.SaveChanges();

            return visitor.VisitorID;
        }

        public void Delete(int id)
        {
            Visitor visitor = Get(id);
            if (visitor != null)
            {
                dataBase.visitors.Remove(visitor);
            }
        }

        public void Update(Visitor visitor)
        {
            var toUpdateVisitor = dataBase.visitors.FirstOrDefault(
                currentVisitor => currentVisitor.VisitorID == visitor.VisitorID);
            if (toUpdateVisitor != null)
            {
                toUpdateVisitor.VisitorID = visitor.VisitorID;
                toUpdateVisitor.Name = visitor.Name ?? toUpdateVisitor.Name;
                toUpdateVisitor.Email = visitor.Email ?? toUpdateVisitor.Email;
                toUpdateVisitor.Password = visitor.Password ??
                    toUpdateVisitor.Password;
            }
        }
    }
}
