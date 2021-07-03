using Microsoft.AspNetCore.Identity;
using Movies.Data;
using Movies.Models;
using Movies.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Managers
{
    

    public class UsuarioManager
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public UsuarioManager
        (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IdentityResult> Register(Usuario model)
        {
            return await _userManager.CreateAsync(new ApplicationUser
            {
                Email = model.Email,
                UserName= model.Email,
                PhoneNumber= model.Phone,
                Name = model.Name,
                LastName = model.LastName,
            },
                model.Password);  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<dynamic> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null) return null;
            // Validate the username/password parameters and ensure the account is not locked out.
            var response = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (response.Succeeded) return user;
            else return response;
        }
    }
}
