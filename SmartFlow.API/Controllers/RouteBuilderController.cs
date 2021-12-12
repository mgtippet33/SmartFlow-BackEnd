using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.API.Models;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SmartFlow.API.Controllers
{
    [Authorize(Roles = "Visitor")]
    [Route("api/route")]
    [ApiController]
    public class RouteBuilderController : ControllerBase
    {
        private IRouteBuilderService routeService;
        private IMapper mapper;

        public RouteBuilderController(IRouteBuilderService routeService)
        {
            this.routeService = routeService;

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

        [HttpGet("{eventID}")]
        public ActionResult<IEnumerable<LocationModel>> Get(int eventID)
        {
            try
            {
                var routeDTO = this.routeService.GetRouteForVisitorByEvent(eventID);
                var route = mapper.Map<IEnumerable<LocationDTO>,
                    List<LocationModel>>(routeDTO);
                return Ok(route);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
