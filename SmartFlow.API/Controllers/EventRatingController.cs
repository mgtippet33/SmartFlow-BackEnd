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
    [Route("api/rating")]
    [ApiController]
    public class EventRatingController : ControllerBase
    {
        private IEventRatingService ratingService;
        private IUserService userService;
        private IEventService eventService;
        private IMapper mapper;

        public EventRatingController(IEventRatingService ratingService,
            IUserService userService,
            IEventService eventService)
        {
            this.ratingService = ratingService;
            this.eventService = eventService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventRatingModel, EventRatingDTO>().ReverseMap();
                    cfg.CreateMap<EventRatingDTO, EventRatingModel>().ReverseMap();
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<EventModel, EventDTO>().ReverseMap();
                    cfg.CreateMap<EventDTO, EventModel>().ReverseMap();
                }
                ).CreateMapper();
        }

        // GET: api/<EventRatingController>
        [HttpGet]
        public ActionResult<EventRatingModel> Get()
        {
            try
            {
                var ratingsDTO = ratingService.GetAllEventRatings();
                var ratings = mapper.Map<IEnumerable<EventRatingDTO>,
                    List<EventRatingModel>>(ratingsDTO);
                return Ok(ratings);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("event/{eventID}")]
        public ActionResult<IEnumerable<EventRatingModel>> GetAverageRatingByEvent(int eventID)
        {
            try
            {
                var currentEvent = eventService.GetEvent(eventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }

                var ratingDTO = ratingService.GetRatingByEvent(eventID);
                var rating = mapper.Map<EventRatingDTO,
                    EventRatingModel>(ratingDTO);
                return Ok(rating);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET api/<EventRatingController>/5
        [HttpGet("{id}")]
        public ActionResult<EventRatingModel> Get(int id)
        {
            try
            {
                var ratingDTO = ratingService.GetEventRating(id);
                if (ratingDTO == null)
                {
                    return NotFound();
                }

                var rating = mapper.Map<EventRatingDTO,
                    EventRatingModel>(ratingDTO);
                return Ok(rating);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/<EventRatingController>
        [Authorize(Roles = "Visitor")]
        [HttpPost]
        public ActionResult<EventRatingModel> Post([FromBody] EventRatingModel model)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                var user = userService.GetUser(Convert.ToInt32(userID));

                var currentEvent = eventService.GetEvent(model.EventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }
                if (InvalidRatingModel(model))
                {
                    return BadRequest("Fill all necessary fields.");
                }

                var ratingDTO = mapper.Map<EventRatingModel, EventRatingDTO>(model);
                ratingDTO.Event = currentEvent;
                ratingDTO.Visitor = user;
                ratingDTO.ScoreDate = DateTime.Now;
                ratingService.AddEventRating(ratingDTO);
                return Ok("Rating added successfully.");
            }
            catch (ArgumentException)
            {
                return BadRequest("This user has already rated the event.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool InvalidRatingModel(EventRatingModel model)
        {
            if (model == null  || model.Score < 0 || model.Score > 5)
            {
                return true;
            }
            return false;
        }

        // PUT api/<EventRatingController>/5
        [Authorize(Roles = "Visitor")]
        [HttpPut("{id}")]
        public ActionResult<EventRatingModel> Put(int id, [FromBody] EventRatingModel model)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }
                var user = userService.GetUser(Convert.ToInt32(userID));

                var currentEvent = eventService.GetEvent(model.EventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }

                var ratingDTO = mapper.Map<EventRatingModel, EventRatingDTO>(model);
                if (ratingDTO.VisitorID != user.UserID)
                {
                    return BadRequest("You can't updated someone else's rating.");
                }
                ratingDTO.EventRatingID = id;
                ratingDTO.Event = currentEvent;
                ratingDTO.Visitor = user;
                ratingService.UpdateEventRating(ratingDTO);
                return Ok("Rating updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<EventRatingController>/5
        [Authorize(Roles = "Visitor")]
        [HttpDelete("{id}")]
        public ActionResult<EventRatingModel> Delete(int id)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var rating = ratingService.GetEventRating(id);
                if (rating == null)
                {
                    return NotFound("This rating does not exist.");
                }

                if (rating.VisitorID != Convert.ToInt32(userID))
                {
                    return BadRequest("You can't delete someone else's rating.");
                }
                ratingService.DeleteEventRating(id);
                return Ok("Rating deleted successfully.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
