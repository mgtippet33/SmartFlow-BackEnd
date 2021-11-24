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
    [Authorize]
    [Route("api/history")]
    [ApiController]
    public class HistoryLocationController : ControllerBase
    {
        private IHistoryLocationService historyService;
        private ILocationService locationService;
        private IUserService userService;
        private IMapper mapper;

        public HistoryLocationController(ILocationService locationService,
            IHistoryLocationService historyService,
            IUserService userService)
        {
            this.historyService = historyService;
            this.locationService = locationService;
            this.userService = userService;

            mapper = mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<HistoryLocationModel,
                        HistoryLocationDTO>().ReverseMap();
                    cfg.CreateMap<HistoryLocationDTO,
                        HistoryLocationModel>().ReverseMap();
                    cfg.CreateMap<LocationModel, LocationDTO>().ReverseMap();
                    cfg.CreateMap<LocationDTO, LocationModel>().ReverseMap();
                    cfg.CreateMap<EventModel, EventDTO>().ReverseMap();
                    cfg.CreateMap<EventDTO, EventModel>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                }
                ).CreateMapper();
        }
        // GET: api/<HistoryLocationController>
        [HttpGet]
        public ActionResult<IEnumerable<HistoryLocationModel>> Get()
        {
            try
            {
                var historiesDTO = historyService.GetAllHistoryLocations();
                var histories = mapper.Map<IEnumerable<HistoryLocationDTO>,
                    List<HistoryLocationModel>>(historiesDTO);
                return Ok(histories);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<HistoryLocationController>/5
        [HttpGet("{id}")]
        public ActionResult<HistoryLocationModel> Get(int id)
        {
            try
            {
                var historyDTO = historyService.GetHistoryLocation(id);
                if (historyDTO == null)
                {
                    return NotFound();
                }

                var history = mapper.Map<HistoryLocationDTO,
                    HistoryLocationModel>(historyDTO);
                return Ok(history);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/<HistoryLocationController>
        [HttpPost]
        public ActionResult<HistoryLocationModel> Post(
            [FromBody] HistoryLocationModel model)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                var user = userService.GetUser(Convert.ToInt32(userID));

                var location = locationService.GetLocation(model.LocationID);
                if (location == null)
                {
                    return NotFound("There is no location with this locationID.");
                }
                if (InvalidHistoryModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var historyDTO = mapper.Map<HistoryLocationModel,
                    HistoryLocationDTO>(model);
                historyDTO.Location = location;
                historyDTO.Visitor = user;
                historyDTO.ActionTime = DateTime.Now;
                historyService.AddHistoryLocation(historyDTO);
                return Ok("History added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("This user action has already been recorded.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidHistoryModel(HistoryLocationModel model)
        {
            if (model == null || model.Action == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<HistoryLocationController>/5
        [HttpPut("{id}")]
        public ActionResult<HistoryLocationModel> Put(int id,
            [FromBody] HistoryLocationModel model)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                var user = userService.GetUser(Convert.ToInt32(userID));

                var location = locationService.GetLocation(model.LocationID);
                if (location == null)
                {
                    return NotFound("There is no location with this locationID.");
                }

                var historyDTO = mapper.Map<HistoryLocationModel,
                    HistoryLocationDTO>(model);
                historyDTO.HistoryLocationID = id;
                historyDTO.Location = location;
                historyDTO.Visitor = user;
                historyService.UpdateHistoryLocation(historyDTO);
                return Ok("History updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<HistoryLocationController>/5
        [HttpDelete("{id}")]
        public ActionResult<HistoryLocationModel> Delete(int id)
        {
            try
            {
                var history = historyService.GetHistoryLocation(id);
                if (history != null)
                {
                    historyService.DeleteHistoryLocation(id);
                    return Ok("History deleted successfully.");
                }
                return NotFound("This history does not exist.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
