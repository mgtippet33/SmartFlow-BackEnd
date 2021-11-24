using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.API.Models;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SmartFlow.API.Controllers
{
    [Authorize(Roles = "Administrator,BusinessPartner")]
    [Route("api/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private IItemService itemService;
        private ILocationService locationService;
        private IMapper mapper;

        public ItemController(IItemService itemService,
            ILocationService locationService)
        {
            this.itemService = itemService;
            this.locationService = locationService;

            mapper = new MapperConfiguration(
               cfg =>
               {
                   cfg.CreateMap<ItemModel, ItemDTO>().ReverseMap();
                   cfg.CreateMap<ItemDTO, ItemModel>().ReverseMap();
                   cfg.CreateMap<LocationModel, LocationDTO>().ReverseMap();
                   cfg.CreateMap<LocationDTO, LocationModel>().ReverseMap();
                   cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                   cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
               })
               .CreateMapper();
        }

        // GET: api/<ItemController>
        [HttpGet]
        public ActionResult<IEnumerable<ItemModel>> Get()
        {
            try
            {
                var itemsDTO = itemService.GetAllItems();
                var items = mapper.Map<IEnumerable<ItemDTO>,
                    List<ItemModel>>(itemsDTO);
                return Ok(items);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("location/{locationID}")]
        public ActionResult<IEnumerable<ItemModel>> GetItemsByLocation(int locationID)
        {
            try
            {
                var itemsDTO = itemService.GetItemsByLocation(locationID);
                var items = mapper.Map<IEnumerable<ItemDTO>,
                    List<ItemModel>>(itemsDTO);
                return Ok(items);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ItemModel> Get(int id)
        {
            try
            {
                var itemDTO = itemService.GetItem(id);
                if (itemDTO == null)
                {
                    return NotFound();
                }

                var item = mapper.Map<ItemDTO,
                    ItemModel>(itemDTO);
                return Ok(item);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/<ItemController>
        [HttpPost]
        public ActionResult<ItemModel> Post([FromBody] ItemModel model)
        {
            try
            {
                var location = locationService.GetLocation(model.LocationID);
                if (location == null)
                {
                    return NotFound("There is no location with this locationID.");
                }
                if (InvalidItemModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var itemDTO = mapper.Map<ItemModel, ItemDTO>(model);
                itemDTO.Location = location;
                itemService.AddItem(itemDTO);
                return Ok("Item added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("A item with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidItemModel(ItemModel model)
        {
            if (model == null || model.Name == null ||
                model.Description == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public ActionResult<ItemModel> Put(int id, [FromBody] ItemModel model)
        {
            try
            {
                var location = locationService.GetLocation(model.LocationID);
                if (location == null)
                {
                    return NotFound("There is no location with this locationID.");
                }

                if (model == null)
                {
                    return BadRequest("Specify the data you want to change");
                }

                var itemDTO = mapper.Map<ItemModel, ItemDTO>(model);
                itemDTO.ItemID = id;
                itemDTO.Location = location;
                itemService.UpdateItem(itemDTO);
                return Ok("Item updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public ActionResult<ItemModel> Delete(int id)
        {
            try
            {
                var item = itemService.GetItem(id);
                if (item != null)
                {
                    itemService.DeleteItem(id);
                    return Ok("Item deleted successfully.");
                }
                return NotFound("This item does not exist.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
