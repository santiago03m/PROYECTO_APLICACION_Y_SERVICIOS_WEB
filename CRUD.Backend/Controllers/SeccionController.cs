using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeccionController : ControllerBase
    {
        private readonly DataContext _context;

        public SeccionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Seccion.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Seccion seccion)
        {
            try
            {
                _context.Seccion.Add(seccion);
                await _context.SaveChangesAsync();
                return Ok(seccion);
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
            var seccion = await _context.Seccion.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }
            return Ok(seccion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Seccion seccion)
        {
            if (id != seccion.Id)
            {
                return BadRequest();
            }

            var seccionExistente = await _context.Seccion.FindAsync(id);
            if (seccionExistente == null)
            {
                return NotFound();
            }

            _context.Entry(seccionExistente).CurrentValues.SetValues(seccion);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(seccion);
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
            var seccion = await _context.Seccion.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }
            _context.Seccion.Remove(seccion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
