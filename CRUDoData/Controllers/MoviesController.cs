using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDoData.Data;
using CRUDoData.Models;
using Microsoft.AspNet.OData;
using System.Net;

namespace CRUDoData.Controllers
{
    public class MoviesController : ODataController
    {
        private readonly AppDbContext context;

        public MoviesController(AppDbContext context) => this.context = context;


        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(context.Movies);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(context.Movies.FirstOrDefault(c => c.Id == key));
        }

        public async Task<IActionResult> Post([FromBody]Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Movies.Add(movie);
            await context.SaveChangesAsync();
            return Created(movie);
        }

        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Movie> movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await context.Movies.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            movie.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri]int key, [FromBody] Movie update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            context.Entry(update).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        public async Task<ActionResult> Delete([FromODataUri] int key)
        {
            var movie = await context.Movies.FindAsync(key);
            if (movie == null)
            {
                return NotFound();
            }
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        private bool MovieExists(int key)
        {
            return context.Movies.Any(x => x.Id == key);
        }

    }
}
