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
    public class BusinessPartnerService : IBusinessPartnerService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public BusinessPartnerService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<BusinessPartner, BusinessPartnerDTO>()
                .ReverseMap()).CreateMapper();
            fromDTOMapper = new MapperConfiguration(
                cfg => cfg.CreateMap<BusinessPartnerDTO, BusinessPartner>()
                .ReverseMap()).CreateMapper();
        }

        public BusinessPartnerDTO GetBusinessPartner(int id)
        {
            var partner = database.BusinessPartners.Get(id);
            if (partner == null)
                throw new ArgumentNullException();
            var partnerDTO = toDTOMapper
                .Map<BusinessPartner, BusinessPartnerDTO>(partner);

            return partnerDTO;
        }

        public IEnumerable<BusinessPartnerDTO> GetAllBusinessPartners()
        {
            var partners = database.BusinessPartners.GetAll();
            var partnersDTO = toDTOMapper.Map<IEnumerable<BusinessPartner>,
                List<BusinessPartnerDTO>>(partners);

            return partnersDTO;
        }

        public int AddBusinessPartner(BusinessPartnerDTO businessPartnerDTO)
        {
            throw new NotImplementedException();
            // TO DO Identity
        }

        public void DeleteBusinessPartner(int id)
        {
            var partner = database.BusinessPartners.Get(id);
            if (partner == null)
                throw new NullReferenceException();

            database.BusinessPartners.Delete(id);
            database.Save();
        }

        public void UpdateBusinessPartner(BusinessPartnerDTO businessPartnerDTO)
        {
            var partner = database.BusinessPartners
                .Get(businessPartnerDTO.BusinessPartnerID);
            if (partner == null)
                throw new NullReferenceException();
            var partnerExsist = database.BusinessPartners.GetAll()
                .Any(currentPartner =>
                    currentPartner.Email == businessPartnerDTO.Email &&
                    currentPartner.BusinessPartnerID ==
                    businessPartnerDTO.BusinessPartnerID);
            if (partnerExsist)
                throw new NullReferenceException();

            partner = fromDTOMapper.Map<BusinessPartnerDTO,
                BusinessPartner>(businessPartnerDTO);
            database.BusinessPartners.Update(partner);
            database.Save();
        }
    }
}
