using AutoMapper;
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
        private readonly IConfiguration config;

        public AdministratorController(IUserService service,
            SignInManager<User> signInManager, UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            ITokenService tokenService, IConfiguration config)
        {
            this.service = service;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.tokenService = tokenService;
            this.config = config;

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

        // POST api/<AdministratorController>
        //[AllowAnonymous]
        //[Route("signIn")]
        //[HttpPost]
        //public async Task<ActionResult<UserModel>> SignIn(UserModel administrator)
        //{
        //    try
        //    {
        //        //if (string.IsNullOrEmpty(administrator.Email) ||
        //        //    string.IsNullOrEmpty(administrator.Password))
        //        //{
        //        //    return NotFound();
        //        //}

        //        //IActionResult response = Unauthorized();
        //        //var result = signInManager.PasswordSignInAsync(
        //        //    administrator.Email, administrator.Password,
        //        //    false, lockoutOnFailure: false);

        //        //if (result.Status == TaskStatus.RanToCompletion)
        //        //{
        //        //    var generatedToken = tokenService.BuildToken(config["Jwt:Key"].ToString(),
        //        //        config["Jwt:Issuer"].ToString(), );

        //        //    if (generatedToken != null)
        //        //    {
        //        //        HttpContext.Session.SetString("Token", generatedToken);
        //        //        return RedirectToAction("MainWindow");
        //        //    }
        //        //    else
        //        //    {
        //        //        return (RedirectToAction("Error"));
        //        //    }
        //        //}





        //        //var signInResult =
        //        //    await signInManager.PasswordSignInAsync(administrator.Email, administrator.Password, true, false);

        //        //if (signInResult.Succeeded)
        //        //{
        //        //    //var now = DateTime.UtcNow;
        //        //    //var jwt = new JwtSecurityToken(
        //        //    //        issuer: AuthOptions.ISSUER,
        //        //    //        audience: AuthOptions.AUDIENCE,
        //        //    //        notBefore: now,
        //        //    //        expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
        //        //    //        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        //        //    //var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        //        //    //var response = new
        //        //    //{
        //        //    //    success = true,
        //        //    //    status_code = 200,
        //        //    //    message = "User logged in successfully",
        //        //    //    access_token = encodedJwt
        //        //    //};

        //        //    //return Ok(response);
        //        //}
        //        //else
        //        //{
        //        //    var response = new
        //        //    {
        //        //        success = false,
        //        //        status_code = 404,
        //        //        message = "User is not found",
        //        //        access_token = string.Empty
        //        //    };
        //        //    return NotFound(response);
        //        //}


        //        //var identity = GetIdentity(administrator.Name, password);
        //        //if (identity == null)
        //        //{
        //        //    return BadRequest(new { errorText = "Invalid username or password." });
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}

        [Route("signUp")]
        [HttpPost]
        public async Task<ActionResult<UserModel>> SignUp(UserModel administrator)
        {
            try
            {
                if (administrator == null)
                {
                    return BadRequest();
                }
                var user = mapper.Map<UserModel, User>(administrator);
                user.UserName = administrator.Name;
                var managerResult = await userManager.CreateAsync(user,
                    administrator.Password);

                var userRoles = from role in roleManager.Roles.ToList()
                                where role.Name == "Administrator"
                                select role.Name;
                var roleResult = await userManager.AddToRolesAsync(user, userRoles);

                if(!managerResult.Succeeded && !roleResult.Succeeded)
                {
                    return BadRequest();
                }
                return Created("", administrator);
                //var administratorDTO = mapper.Map<UserModel,
                //    UserDTO>(administrator);
                //service.AddUser(administratorDTO);
                //var response = new TokenResponseModel { }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT api/<AdministratorController>/5
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
