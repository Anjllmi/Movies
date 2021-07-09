using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies.Data;
using Movies.Managers;
using Movies.Models;
using Movies.Models.ViewModels;
using Movies.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    //Incluido para solucionar el error 302 que precedia al 404 en este controlador.
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UsuarioManager _usuarioManager;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, IConfiguration config,UsuarioManager usuarioManager)
        {
            _context = context;
            _config = config;
            _usuarioManager = usuarioManager;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UsuarioViewModel>>> GetAll()
        {
            //return await _context.Categories.ToListAsync();
            return Ok(await _context.Usuarios.ToListAsync());
        }

        //POST api/user/Register
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        //[HttpPost]
        [Authorize(Roles = "ADMIN")]
        //[AllowAnonymous]
        public IActionResult Register(Usuario model)
        {
            //Guarda la contraseña encriptada
            model.Password=model.Password.EncryptHash256();
            _context.Usuarios.Add(model);
            _context.SaveChangesAsync();
            return Ok();
        }

        //POST api/user/RegisterUser
        /// <summary>
        /// Registro de usarios
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        //[HttpPost]
        [Authorize(Roles = "ADMIN")]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterUser(Usuario model)
        {
            var response = await _usuarioManager.Register(model);
            if (!response.Succeeded) 
            {
                response.Errors.ToList().ForEach(x => ModelState.AddModelError(x.Code, x.Description));
                return BadRequest(ModelState);
            }
            
            else return Ok();
        }


        //POST api/user/login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Guarda la contraseña encriptada
            model.Password = model.Password.EncryptHash256();
            var user=await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (user == null) return NotFound(); //404
            if (!user.Password.Equals(model.Password)) 
            {
                ModelState.AddModelError("Error", "Credenciales inválidas");
                return BadRequest(ModelState); //400
            };

            var accesToken = GenerateJWT(user);

            return Ok(new
            {
                AccesToken=accesToken
            }); //200
        }

        //POST api/user/loginUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            var response = await _usuarioManager.Login(model);
            if (response == null) return NotFound(); //404

            if(response is Microsoft.AspNetCore.Identity.SignInResult @sign)

                if (!@sign.Succeeded) {

                    if(@sign.IsLockedOut)ModelState.AddModelError("Error", "IsLockedOut");
                    if (@sign.IsNotAllowed) ModelState.AddModelError("Error", "IsNotAllowed");
                    return BadRequest(ModelState); //400

                }
            

            var accesToken = GenerateJWT(response as ApplicationUser);

            return Ok(new
            {
                AccesToken = accesToken
            }); //200
        }

        #region helper

        /// <summary>
        /// Json Web Token
        /// </summary>
        private string GenerateJWT(Usuario model)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,model.Name),
                new Claim(ClaimTypes.Role,model.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier,model.Email),
            };

            var key = Encoding.ASCII.GetBytes(_config["TokenValidationParameters:IssuerSingingKey"]);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);



            var token = new JwtSecurityToken(
                _config["TokenValidationParameters:Issuer"],
                _config["TokenValidationParameters:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Json Web Token
        /// </summary>
        private string GenerateJWT(IdentityUser model)
        {
            var claims = new[]
            { 
                new Claim(ClaimTypes.NameIdentifier,model.Email),
            };

            var key = Encoding.ASCII.GetBytes(_config["TokenValidationParameters:IssuerSingingKey"]);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);



            var token = new JwtSecurityToken(
                _config["TokenValidationParameters:Issuer"],
                _config["TokenValidationParameters:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}
