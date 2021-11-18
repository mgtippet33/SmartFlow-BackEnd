using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IVisitorService
    {
        IEnumerable<VisitorDTO> GetAllVisitors();
        VisitorDTO GetVisitor(int id);
        int AddVisitor(VisitorDTO visitorDTO);
        void DeleteVisitor(int id);
        void UpdateVisitor(VisitorDTO visitorDTO);
    }
}
