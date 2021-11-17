using Microsoft.EntityFrameworkCore;
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
    public class ItemRepository : IRepository<Item>
    {
        private SmartFlowContext dataBase;

        public ItemRepository(SmartFlowContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public Item Get(int id)
        {
            return dataBase.items
                .Include(item => item.Location)
                .SingleOrDefault(item => item.ItemID == id);
        }

        public IEnumerable<Item> GetAll()
        {
            return dataBase.items.Include(item => item.Location).ToList();
        }

        public int Create(Item item)
        {
            item.Location = dataBase.locations
                .Find(item.Location.LocationID);
            dataBase.items.Add(item);
            dataBase.SaveChanges();

            return item.ItemID;
        }

        public void Delete(int id)
        {
            Item item = Get(id);
            if (item != null)
            {
                dataBase.items.Remove(item);
            }
        }

        public void Update(Item item)
        {
            var toUpdateItem = dataBase.items.FirstOrDefault(
                itm => itm.ItemID == item.ItemID);
            if (toUpdateItem != null)
            {
                toUpdateItem.ItemID = item.ItemID;
                toUpdateItem.Location = dataBase.locations.
                    Find(item.Location.LocationID);
                toUpdateItem.Name = item.Name ?? toUpdateItem.Name;
                toUpdateItem.Description = item.Description ?? toUpdateItem.Description;
                toUpdateItem.Link = item.Link ?? toUpdateItem.Link;
            }
        }
    }
}
