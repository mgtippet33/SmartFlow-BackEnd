using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IBusinessPartnerService
    {
        IEnumerable<BusinessPartnerDTO> GetAllBusinessPartners();
        BusinessPartnerDTO GetBusinessPartner(int id);
        int AddBusinessPartner(BusinessPartnerDTO businessPartnerDTO);
        void DeleteBusinessPartner(int id);
        void UpdateBusinessPartner(BusinessPartnerDTO businessPartnerDTO);
    }
}
