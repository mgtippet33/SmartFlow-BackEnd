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
        private IMapper mapper;
        private IWorkUnit database;

        public ItemService(IWorkUnit database)
        {
            this.database = database;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<Item, ItemDTO>().ReverseMap();
                    cfg.CreateMap<ItemDTO, Item>().ReverseMap();
                    cfg.CreateMap<Location, LocationDTO>().ReverseMap();
                    cfg.CreateMap<LocationDTO, Location>().ReverseMap();
                    cfg.CreateMap<Event, EventDTO>().ReverseMap();
                    cfg.CreateMap<EventDTO, Event>().ReverseMap();
                    cfg.CreateMap<User, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, User>().ReverseMap();
                }
                ).CreateMapper();
        }

        public IEnumerable<ItemDTO> GetAllItems()
        {
            var items = database.Items.GetAll()
                .OrderBy(item => item.ItemID);
            var itemsDTO = mapper.Map<IEnumerable<Item>,
                List<ItemDTO>>(items);

            return itemsDTO;
        }

        public IEnumerable<ItemDTO> GetItemsByLocation(int locationID)
        {
            var items = database.Items.GetAll();
            items = items.Where(item =>
                item.Location.LocationID == locationID)
                .OrderBy(item => item.ItemID)
                .ToList();
            var itemsDTO = mapper.Map<IEnumerable<Item>,
                List<ItemDTO>>(items);

            return itemsDTO;
        }

        public ItemDTO GetItem(int id)
        {
            var item = database.Items.Get(id);
            if (item == null)
                throw new NullReferenceException();
            var itemDTO = mapper
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

            var item = mapper.Map<ItemDTO, Item>(itemDTO);
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
                    itm.ItemID != itemDTO.ItemID &&
                    itm.Name == itemDTO.Name &&
                    itm.Location.LocationID == itemDTO.Location.LocationID);
            if (itemExsist)
                throw new NullReferenceException();

            item = mapper.Map<ItemDTO, Item>(itemDTO);
            database.Items.Update(item);
            database.Save();
        }
    }
}
