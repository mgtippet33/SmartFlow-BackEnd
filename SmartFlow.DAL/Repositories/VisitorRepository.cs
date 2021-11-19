//using SmartFlow.DAL.EF;
//using SmartFlow.DAL.Entities;
//using SmartFlow.DAL.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartFlow.DAL.Repositories
//{
//    public class VisitorRepository : IRepository<Visitor>
//    {
//        private SmartFlowContext database;

//        public VisitorRepository(SmartFlowContext database)
//        {
//            this.database = database;
//        }

//        public Visitor Get(int id)
//        {
//            return database.visitors.Find(id);
//        }

//        public IEnumerable<Visitor> GetAll()
//        {
//            return database.visitors.ToList();
//        }

//        public int Create(Visitor visitor)
//        {
//            database.visitors.Add(visitor);
//            database.SaveChanges();

//            return visitor.VisitorID;
//        }

//        public void Delete(int id)
//        {
//            Visitor visitor = Get(id);
//            if (visitor != null)
//            {
//                database.visitors.Remove(visitor);
//            }
//        }

//        public void Update(Visitor visitor)
//        {
//            var toUpdateVisitor = database.visitors.FirstOrDefault(
//                currentVisitor => currentVisitor.VisitorID == visitor.VisitorID);
//            if (toUpdateVisitor != null)
//            {
//                toUpdateVisitor.VisitorID = visitor.VisitorID;
//                toUpdateVisitor.Name = visitor.Name ?? toUpdateVisitor.Name;
//                toUpdateVisitor.Email = visitor.Email ?? toUpdateVisitor.Email;
//                toUpdateVisitor.Password = visitor.Password ??
//                    toUpdateVisitor.Password;
//            }
//        }
//    }
//}
