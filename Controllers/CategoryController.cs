using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Models;
using Movies.Services;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericCRUD<Category> _service;

        public CategoryController(IGenericCRUD<Category> service)
        {
            _service = service;
        }

        // GET: api/Category
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            //return await _context.Categories.ToListAsync();
            return Ok(await _service.GetAll());
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            //var category = await _context.Categories.FindAsync(id);

            var category = await _service.GetDetails(id);
            if (category == null) return NotFound();
            return category;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            category = await _service.Update(id,category);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            //_context.Categories.Add(category);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction("GetCategory", new { id = category.Id }, category);

            return Ok(await _service.Create(category));


        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _service.Delete(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
    }
}
