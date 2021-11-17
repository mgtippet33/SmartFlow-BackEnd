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
    public class BusinessPartnerRepository : IRepository<BusinessPartner>
    {
        private SmartFlowContext dataBase;

        public BusinessPartnerRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public BusinessPartner Get(int id)
        {
            return dataBase.businessPartners.Find(id);
        }

        public IEnumerable<BusinessPartner> GetAll()
        {
            return dataBase.businessPartners.ToList();
        }

        public int Create(BusinessPartner businessPartner)
        {
            dataBase.businessPartners.Add(businessPartner);
            dataBase.SaveChanges();

            return businessPartner.BusinessPartnerID;
        }

        public void Delete(int id)
        {
            BusinessPartner businessPartner = Get(id);
            if (businessPartner != null)
            {
                dataBase.businessPartners.Remove(businessPartner);
            }
        }

        public void Update(BusinessPartner businessPartner)
        {
            var toUpdatePartner = dataBase.businessPartners.FirstOrDefault(
                partner => partner.BusinessPartnerID == businessPartner.BusinessPartnerID);
            if (toUpdatePartner != null)
            {
                toUpdatePartner.BusinessPartnerID = businessPartner.BusinessPartnerID;
                toUpdatePartner.Name = businessPartner.Name ?? toUpdatePartner.Name;
                toUpdatePartner.Email = businessPartner.Email ?? toUpdatePartner.Email;
                toUpdatePartner.Password = businessPartner.Password ?? toUpdatePartner.Password;
            }
        }
    }
}
