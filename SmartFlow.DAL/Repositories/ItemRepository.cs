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
        private SmartFlowContext database;

        public ItemRepository(SmartFlowContext database)
        {
            this.database = database;
        }

        public Item Get(int id)
        {
            return database.items
                .Include(item => item.Location)
                .SingleOrDefault(item => item.ItemID == id);
        }

        public IEnumerable<Item> GetAll()
        {
            return database.items.Include(item => item.Location).ToList();
        }

        public int Create(Item item)
        {
            item.Location = database.locations
                .Find(item.Location.LocationID);
            database.items.Add(item);
            database.SaveChanges();

            return item.ItemID;
        }

        public void Delete(int id)
        {
            Item item = Get(id);
            if (item != null)
            {
                database.items.Remove(item);
            }
        }

        public void Update(Item item)
        {
            var toUpdateItem = database.items.FirstOrDefault(
                itm => itm.ItemID == item.ItemID);
            if (toUpdateItem != null)
            {
                toUpdateItem.ItemID = item.ItemID;
                toUpdateItem.Location = database.locations.
                    Find(item.Location.LocationID);
                toUpdateItem.Name = item.Name ?? toUpdateItem.Name;
                toUpdateItem.Description = item.Description ?? toUpdateItem.Description;
                toUpdateItem.Link = item.Link ?? toUpdateItem.Link;
            }
        }
    }
}
