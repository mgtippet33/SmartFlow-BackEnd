using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartFlow.API.Models;
using SmartFlow.BLL.DTO;
using SmartFlow.BLL.Interfaces;
using SmartFlow.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;


namespace SmartFlow.API.Controllers
{
    [Route("api/administrator")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private IUserService service;
        private IMapper mapper;
        SignInManager<User> signInManager;
        UserManager<User> userManager;
        RoleManager<IdentityRole<int>> roleManager;
        private readonly ITokenService tokenService;

        public AdministratorController(IUserService service,
            SignInManager<User> signInManager, UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ITokenService tokenService)
        {
            this.service = service;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;

            mapper = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<UserModel, UserDTO>().ReverseMap();
                    cfg.CreateMap<UserDTO, UserModel>().ReverseMap();
                    cfg.CreateMap<UserModel, User>().ReverseMap();
                })
                .CreateMapper();

        }

        // GET: api/<AdministratorController>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            try
            {
                var administratorsDTO = service.GetAllUsers();
                var administrators = mapper.Map<IEnumerable<UserDTO>,
                    List<UserModel>>(administratorsDTO);
                return Ok(administrators);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public ActionResult<UserModel> Get(int id)
        {
            try
            {
                var administratorDTO = service.GetUser(id);
                if (administratorDTO == null)
                {
                    return NotFound();
                }
                var administrator = mapper.Map<UserDTO,
                    UserModel>(administratorDTO);
                return Ok(administrator);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UserModel model)
        {
            try
            {
                var administrator = service.GetUser(id);
                if (administrator != null)
                {
                    var administratorDTO = mapper.Map<UserModel, UserDTO>(model);
                    administratorDTO.UserID = id;
                    service.UpdateUser(administratorDTO);
                    return Ok();
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // DELETE api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var administrator = service.GetUser(id);
                if (administrator != null)
                {
                    service.DeleteUser(id);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
