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
    public class ItemService : IItemService
    {
        private IMapper toDTOMapper;
        private IMapper fromDTOMapper;
        private IWorkUnit database;

        public ItemService(IWorkUnit database)
        {
            this.database = database;

            toDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Item, ItemDTO>().ReverseMap();
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                }
                ).CreateMapper();

            fromDTOMapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<ItemDTO, Item>().ReverseMap();
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                }
                ).CreateMapper();

        }

        public IEnumerable<ItemDTO> GetAllItems()
        {
            var items = database.Items.GetAll();
            var itemsDTO = toDTOMapper.Map<IEnumerable<Item>,
                List<ItemDTO>>(items);

            return itemsDTO;
        }

        public ItemDTO GetItem(int id)
        {
            var item = database.Items.Get(id);
            if (item == null)
                throw new NullReferenceException();
            var itemDTO = toDTOMapper
                .Map<Item, ItemDTO>(item);

            return itemDTO;
        }

        public int AddItem(ItemDTO itemDTO)
        {
            if (itemDTO.Location == null)
                throw new ArgumentNullException();
            var itemExsist = database.Items.GetAll()
                .Any(itm => itm.Name == itemDTO.Name &&
                    itm.Location.LocationID == itemDTO.Location.LocationID);
            if (itemExsist)
                throw new ArgumentException();

            var item = fromDTOMapper.Map<ItemDTO, Item>(itemDTO);
            var itemID = database.Items.Create(item);
            return itemID;
        }

        public void DeleteItem(int id)
        {
            var item = database.Items.Get(id);
            if (item == null)
                throw new NullReferenceException();

            database.Items.Delete(id);
            database.Save();
        }

        public void UpdateItem(ItemDTO itemDTO)
        {
            var item = database.Items.Get(itemDTO.ItemID);
            if (item == null)
                throw new NullReferenceException();
            var itemExsist = database.Items.GetAll()
                .Any(itm =>
                    itm.Name == itemDTO.Name &&
                    itm.Location.LocationID == itemDTO.Location.LocationID);
            if (itemExsist)
                throw new NullReferenceException();

            item = fromDTOMapper.Map<ItemDTO, Item>(itemDTO);
            database.Items.Update(item);
            database.Save();
        }
    }
}
