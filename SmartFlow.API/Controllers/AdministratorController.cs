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
        private IMigrationService migrationService;
        UserManager<User> userManager;

        public AdministratorController(IUserService service,
            UserManager<User> userManager, IMigrationService migrationService)
        {
            this.service = service;
            this.userManager = userManager;
            this.migrationService = migrationService;

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
        [HttpGet("{role}")]
        public ActionResult<IEnumerable<UserModel>> Get(string role)
        {
            try
            {
                switch (role)
                {
                    case "visitor":
                        role = "Visitor";
                        break;
                    case "businessPartner":
                        role = "BusinessPartner";
                        break;
                    default:
                        role = "Administrator";
                        break;
                }

                var usersDTO = service.GetUsersOfOneRole(role);
                var users = mapper.Map<IEnumerable<UserDTO>,
                    List<UserModel>>(usersDTO);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/<AdministratorController>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            try
            {
                var usersDTO = service.GetAllUsers();
                var users = mapper.Map<IEnumerable<UserDTO>,
                    List<UserModel>>(usersDTO);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id:int}")]
        public ActionResult<UserModel> Get(int id)
        {
            try
            {
                var userDTO = service.GetUser(id);
                if (userDTO == null)
                {
                    return NotFound();
                }
                var user = mapper.Map<UserDTO, UserModel>(userDTO);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpPut("{userID}")]
        public async Task<ActionResult<UserModel>> Put(int userID, 
            [FromBody] UserModel model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userID.ToString());
                if (model.Name != null && user.Name != model.Name)
                {
                    user.Name = model.Name;
                    await userManager.UpdateAsync(user);
                }

                if (model.Email != null && user.Email != model.Email)
                {
                    user.Email = model.Email;
                    await userManager.UpdateAsync(user);
                }

                if (model.Password != null)
                {
                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, model.Password);
                }

                return Ok(new
                {
                    status = 200,
                    message = "User profile updated successfully"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE api/<AdministratorController>/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = service.GetUser(id);
                if (user != null)
                {
                    service.DeleteUser(id);
                    return Ok(new {
                        status = 200,
                        message = "User deleted successfully."
                    });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Administrator")]
        [Route("~/api/backup")]
        [HttpGet]
        public ActionResult BackupDatabase()
        {
            try
            {
                var migration = migrationService.BackupDatabase();
                if (migration.Length != 0)
                    return Ok(new
                    {
                        status = 200,
                        message = migration
                    });
                return BadRequest(new
                {
                    status = 400,
                    message = migration
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [Route("~/api/restore")]
        public ActionResult RestoreDatabase([FromBody] RestoreModel restore)
        {
            try
            {
                var migrationStatus = migrationService.RestoreDatabase(restore.RestoreText);
                if (migrationStatus)
                    return Ok(new
                    {
                        status = 200,
                        message = "Restore was made successfully."
                    });
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
