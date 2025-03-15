using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadMedicionController : ControllerBase
    {
        private readonly DataContext _context;

        public UnidadMedicionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.UnidadMedicion.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(UnidadMedicion unidadMedicion)
        {
            try
            {
                _context.UnidadMedicion.Add(unidadMedicion);
                await _context.SaveChangesAsync();
                return Ok(unidadMedicion);
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
        public async Task<ActionResult> GetAsync(int id)
        {
            var unidadMedicion = await _context.UnidadMedicion.FindAsync(id);
            if (unidadMedicion == null)
            {
                return NotFound();
            }
            return Ok(unidadMedicion);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, UnidadMedicion unidadMedicion)
        {
            if (id != unidadMedicion.Id)
            {
                return BadRequest();
            }

            var unidadMedicionExistente = await _context.UnidadMedicion.FindAsync(id);
            if (unidadMedicionExistente == null)
            {
                return NotFound();
            }

            _context.Entry(unidadMedicionExistente).CurrentValues.SetValues(unidadMedicion);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(unidadMedicion);
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var unidadMedicion = await _context.UnidadMedicion.FindAsync(id);
            if (unidadMedicion == null)
            {
                return NotFound();
            }
            _context.UnidadMedicion.Remove(unidadMedicion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
