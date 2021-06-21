using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Data;
using Movies.Models;
using Movies.Services;
using Movies.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Controllers
{

    //controller es reemplazado por el nombre de la clase MovieController pero solo «Movie».
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MovieController : ControllerBase
    {
        private readonly ILogger _logger;

        //El símbolo de  _ es para notar propiedades privadas de la clase.
        //Ya no usamos Singleton porque usamos la persistencia de la Base de datos.
        //private readonly MovieSingleton _movieSingleton;

        private readonly ApplicationDbContext _context;
        private readonly IGenericCRUD<Movie> _service;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="movieSingleton"></param>
        public MovieController(/*MovieSingleton movieSingleton,*/ ILogger<MovieController> logger, ApplicationDbContext context,IGenericCRUD<Movie> service)
        {
            //_movieSingleton = movieSingleton;
            _context = context;
            _logger = logger;
            _service = service;
        }


        //GET: api/movie/
        [HttpGet]
        //[AllowAnonymous]
        [Authorize(Roles = "ADMIN")]
        public IEnumerable<Movie> All(int? year)
        {
            //Información del año de la película
            _logger.LogInformation($"year{year} user{HttpContext.User}");

            //throw new Exception(); //"toteador"

            //return year!=null? _movieSingleton.All.Where(x => x.Year == year):_movieSingleton.All;

            //la relación include inluye todas las relaciones
            return year != null ? _context.Movies
                .Include(x => x.Category).Where(x => x.Year == year).ToList() : _context.Movies
                .Include(x=> x.Category)
                .ToList();
        }

        // PUT: api/Movie/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            movie = await _service.Update(id, movie);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        // POST: api/Movie
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            //_context.Categories.Add(category);
            //await _context.SaveChangesAsync(); 
            //return CreatedAtAction("GetCategory", new { id = category.Id }, category);

            return Ok(await _service.Create(movie));


        }

    }
}
