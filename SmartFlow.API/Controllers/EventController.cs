using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.API.Models;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartFlow.API.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService eventService;
        private IUserService userService;
        private IMapper mapper;

        public EventController(IEventService eventService,
            IUserService userService)
        {
            this.eventService = eventService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventModel, EventDTO>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<EventDTO, EventModel>().ReverseMap();
                })
                .CreateMapper();
        }

        // GET: api/<EventController>
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<EventModel>> Get()
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                var eventsDTO = eventService.GetAllEvents(Convert.ToInt32(userID));
                var events = mapper.Map<IEnumerable<EventDTO>,
                    List<EventModel>>(eventsDTO);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<EventController>/5
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<EventModel> Get(int id)
        {
            try
            {
                var eventDTO = eventService.GetEvent(id);
                if (eventDTO == null)
                {
                    return NotFound();
                }

                var currentEvent = mapper.Map<EventDTO,EventModel>(eventDTO);
                return Ok(currentEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST api/<EventController>
        [Authorize(Roles = "Administrator,BusinessPartner")]
        [HttpPost]
        public ActionResult<EventModel> Post([FromBody] EventModel model)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                if (InvalidEventModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var eventDTO = mapper.Map<EventModel, EventDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(userID));
                eventDTO.BusinessPartner = user;
                eventService.AddEvent(eventDTO);
                return Created("", new
                    {
                        status = 201,
                        message = "Event added successfully.",
                    });
            }
            catch (ArgumentException)
            {
                return BadRequest("An event with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidEventModel(EventModel model)
        {
            if (model == null || model.Name == null ||
                model.Description == null || model.Coordinates == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<EventController>/5
        [Authorize(Roles = "Administrator,BusinessPartner")]
        [HttpPut("{id}")]
        public ActionResult<EventModel> Put(int id, [FromBody] EventModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Specify the data you want to change");
                }

                var modelID = HttpContext.User.Identity!.Name;
                if (modelID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                model.EventID = id;
                var eventDTO = mapper.Map<EventModel, EventDTO>(model);
                var user = userService.GetUser(Convert.ToInt32(modelID));
                eventDTO.BusinessPartner = user;
                eventService.UpdateEvent(eventDTO);
                return Ok(
                    new {
                        status = 200,
                        message = "Event updated successfully.",
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<EventController>/5
        [Authorize(Roles = "Administrator,BusinessPartner")]
        [HttpDelete("{id}")]
        public ActionResult<EventModel> Delete(int id)
        {
            try
            {
                var eventModel = eventService.GetEvent(id);
                if (eventModel != null)
                {
                    eventService.DeleteEvent(id);
                    return Ok(
                        new
                        {
                            status = 200,
                            message = "Event deleted successfully.",
                        });
                }
                return NotFound("This event does not exist.");
            }
            catch (Exception)
            {
                return BadRequest("This event does not exist.");
            }
        }
    }
}
