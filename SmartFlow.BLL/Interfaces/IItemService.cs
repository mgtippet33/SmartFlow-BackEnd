using SmartFlow.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.BLL.Interfaces
{
    public interface IItemService
    {
        IEnumerable<ItemDTO> GetAllItems();
        IEnumerable<ItemDTO> GetItemsByLocation(int locationID);
        ItemDTO GetItem(int id);
        int AddItem(ItemDTO itemDTO);
        void DeleteItem(int id);
        void UpdateItem(ItemDTO itemDTO);
    }
}
