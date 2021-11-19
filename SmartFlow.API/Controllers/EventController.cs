using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
//    [Authorize(Roles = "BusinessPartner")]
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService eventService;
        private ITokenService tokenService;
        private IMapper mapper;

        public EventController(IEventService eventService,
            ITokenService tokenService)
        {
            this.eventService = eventService;
            this.tokenService = tokenService;

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
        [HttpGet]
        public ActionResult<IEnumerable<EventModel>> Get()
        {
            try
            {
                var eventsDTO = eventService.GetAllEvents();
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
        [Authorize]
        [HttpPost]
        public ActionResult<EventModel> Post([FromBody] EventModel model)
        {
            try
            {
                var user = HttpContext.User;
                var id = user.Identity!.Name;
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // PUT api/<EventController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
