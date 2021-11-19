//using AutoMapper;
//using SmartFlow.BLL.DTO;
//using SmartFlow.BLL.Interfaces;
//using SmartFlow.DAL.Entities;
//using SmartFlow.DAL.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartFlow.BLL.Services
//{
//    public class AdministratorService : IAdministratorService
//    {
//        private IMapper toDTOMapper;
//        private IMapper fromDTOMapper;
//        private IWorkUnit database;

//        public AdministratorService(IWorkUnit database)
//        {
//            this.database = database;

//            toDTOMapper = new MapperConfiguration(
//                cfg => cfg.CreateMap<Administrator, AdministratorDTO>()
//                .ReverseMap()).CreateMapper();
//            fromDTOMapper = new MapperConfiguration(
//                cfg => cfg.CreateMap<AdministratorDTO, Administrator>()
//                .ReverseMap()).CreateMapper();
//        }

//        public AdministratorDTO GetAdministrator(int id)
//        {
//            var administrator = database.Administrators.Get(id);
//            if (administrator == null)
//                throw new ArgumentNullException();
//            var administratorDTO = toDTOMapper
//                .Map<Administrator, AdministratorDTO>(administrator);

//            return administratorDTO;
//        }

//        public IEnumerable<AdministratorDTO> GetAllAdministrators()
//        {
//            var administrators = database.Administrators.GetAll();
//            var administratorsDTO = toDTOMapper.Map<IEnumerable<Administrator>,
//                List<AdministratorDTO>>(administrators);

//            return administratorsDTO;
//        }

//        public int AddAdministrator(AdministratorDTO administratorDTO)
//        {
//            var admin = fromDTOMapper.Map<AdministratorDTO, Administrator>(administratorDTO);
//            var adminID = database.Administrators.Create(admin);
//            return adminID;
//        }

//        public void DeleteAdministrator(int id)
//        {
//            var administrator = database.Administrators.Get(id);
//            if (administrator == null)
//                throw new NullReferenceException();

//            database.Administrators.Delete(id);
//            database.Save();
//        }

//        public void UpdateAdministrator(AdministratorDTO administratorDTO)
//        {
//            var administrator = database.Administrators
//                .Get(administratorDTO.AdministratorID);
//            if (administrator == null)
//                throw new NullReferenceException();
//            var adminExsist = database.Administrators.GetAll()
//                .Any(admin => admin.Email == administratorDTO.Email &&
//                    admin.AdministratorID == administratorDTO.AdministratorID);
//            if (adminExsist)
//                throw new NullReferenceException();

//            administrator = fromDTOMapper.Map<AdministratorDTO,
//                Administrator>(administratorDTO);
//            database.Administrators.Update(administrator);
//            database.Save();
//        }
//    }
//}
