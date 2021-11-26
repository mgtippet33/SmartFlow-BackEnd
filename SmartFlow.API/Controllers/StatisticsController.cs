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
using System.Threading.Tasks;

namespace SmartFlow.API.Controllers
{
    [Authorize(Roles = "Administrator,BusinessPartner")]
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IStatisticsService statisticsService;
        private IEventService eventService;
        private IUserService userService;
        private IMapper mapper;

        public StatisticsController(IStatisticsService statisticsService,
            IEventService eventService,
            IUserService userService)
        {
            this.statisticsService = statisticsService;
            this.eventService = eventService;
            this.userService = userService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<EventStatisticDTO,
                        EventStatisticModel>().ReverseMap();
                    cfg.CreateMap<LocationStatisticDTO,
                        LocationStatisticModel>().ReverseMap();
                }
                ).CreateMapper();
        }

        [HttpGet]
        public ActionResult<IEnumerable<EventStatisticModel>> GetEventTop()
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var eventTopDTO = this.statisticsService.GetEventTop(Convert.ToInt32(userID));
                var eventTop = mapper.Map<IEnumerable<EventStatisticDTO>,
                    List<EventStatisticModel>>(eventTopDTO);

                return eventTop;
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{eventID}")]
        public ActionResult<IEnumerable<LocationStatisticModel>> GetLocationStaticsByEvent(int eventID)
        {
            try
            {
                var userID = HttpContext.User.Identity!.Name;
                if (userID == null)
                {
                    return BadRequest("The action is available to authorized users.");
                }

                var currentEvent = eventService.GetEvent(eventID);
                if (currentEvent == null)
                {
                    return NotFound("There is no event with this eventID.");
                }

                if (currentEvent.BusinessPartner.UserID != Convert.ToInt32(userID))
                {
                    return BadRequest("You cannot view the statistics of other people's events.");
                }

                var locationStatisticsDTO =
                    this.statisticsService.GetLocationStatisticsByEvent(eventID);
                var locationStatistics = mapper.Map<IEnumerable<LocationStatisticDTO>,
                    List<LocationStatisticModel>>(locationStatisticsDTO);

                return locationStatistics;
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
