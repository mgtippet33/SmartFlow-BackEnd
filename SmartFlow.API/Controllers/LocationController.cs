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
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private ILocationService locationService;
        private IEventService eventService;
        private IMapper mapper;

        public LocationController(ILocationService locationService,
            IEventService eventService)
        {
            this.locationService = locationService;
            this.eventService = eventService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<LocationModel, LocationDTO>().ReverseMap();
                    cfg.CreateMap<LocationDTO, LocationModel>().ReverseMap();
                    cfg.CreateMap<EventModel, EventDTO>().ReverseMap();
                    cfg.CreateMap<EventDTO, EventModel>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                })
                .CreateMapper();
        }

        // GET: api/<LocationController>
        [HttpGet]
        public ActionResult<IEnumerable<LocationDTO>> Get()
        {
            try
            {
                var locationsDTO = locationService.GetAllLocations();
                var locations = mapper.Map<IEnumerable<LocationDTO>,
                    List<LocationModel>>(locationsDTO);
                return Ok(locations);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("event/{eventID}")]
        public ActionResult<IEnumerable<LocationDTO>> GetLocationsByEvent(int eventID)
        {
            try
            {
                var locationsDTO = locationService.GetLocationsByEvent(eventID);
                var locations = mapper.Map<IEnumerable<LocationDTO>,
                    List<LocationModel>>(locationsDTO);
                return Ok(locations);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<LocationController>/5
        [HttpGet("{id}")]
        public ActionResult<LocationModel> Get(int id)
        {
            try
            {
                var locationDTO = locationService.GetLocation(id);
                if (locationDTO == null)
                {
                    return NotFound();
                }

                var location = mapper.Map<LocationDTO,
                    LocationModel>(locationDTO);
                return Ok(location);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/<LocationController>
        [HttpPost]
        public ActionResult<LocationModel> Post([FromBody] LocationModel model)
        {
            try
            {
                var currentEvent = eventService.GetEvent(model.EventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }
                if (InvalidLocationModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var locationDTO = mapper.Map<LocationModel, LocationDTO>(model);
                locationDTO.Event = currentEvent;
                locationService.AddLocation(locationDTO);
                return Ok("Location added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("A location with the same name already exists.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidLocationModel(LocationModel model)
        {
            if (model == null || model.Name == null ||
                model.Description == null || model.State == null)
            {
                return true;
            }
            return false;
        }

        // PUT api/<LocationController>/5
        [HttpPut("{id}")]
        public ActionResult<LocationModel> Put(int id, [FromBody] LocationModel model)
        {
            try
            {
                var currentEvent = eventService.GetEvent(model.EventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }

                if (model == null)
                {
                    return BadRequest("Specify the data you want to change");
                }

                var locationDTO = mapper.Map<LocationModel, LocationDTO>(model);
                locationDTO.LocationID = id;
                locationDTO.Event = currentEvent;
                locationService.UpdateLocation(locationDTO);
                return Ok("Location updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<LocationController>/5
        [HttpDelete("{id}")]
        public ActionResult<LocationModel> Delete(int id)
        {
            try
            {
                var location = locationService.GetLocation(id);
                if (location != null)
                {
                    locationService.DeleteLocation(id);
                    return Ok("Location deleted successfully.");
                }
                return NotFound("This location does not exist.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
