using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubseccionController : ControllerBase
    {
        private readonly DataContext _context;

        public SubseccionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Subseccion.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(SubSeccion subseccion)
        {
            try
            {
                _context.Subseccion.Add(subseccion);
                await _context.SaveChangesAsync();
                return Ok(subseccion);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id)
        {
            var subseccion = await _context.Subseccion.FindAsync(id);
            if (subseccion == null)
            {
                return NotFound();
            }
            return Ok(subseccion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, SubSeccion subseccion)
        {
            if (id != subseccion.Id)
            {
                return BadRequest();
            }

            var subseccionExistente = await _context.Subseccion.FindAsync(id);
            if (subseccionExistente == null)
            {
                return NotFound();
            }

            _context.Entry(subseccionExistente).CurrentValues.SetValues(subseccion);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(subseccion);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var subseccion = await _context.Subseccion.FindAsync(id);
            if (subseccion == null)
            {
                return NotFound();
            }
            _context.Subseccion.Remove(subseccion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
