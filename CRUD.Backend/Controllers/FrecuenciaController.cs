using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrecuenciaController : ControllerBase
    {
        private readonly DataContext _context;

        public FrecuenciaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Frecuencia.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Frecuencia frecuencia)
        {
            try
            {
                _context.Frecuencia.Add(frecuencia);
                await _context.SaveChangesAsync();
                return Ok(frecuencia);
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
            var frecuencia = await _context.Frecuencia.FindAsync(id);
            if (frecuencia == null)
            {
                return NotFound();
            }
            return Ok(frecuencia);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Frecuencia frecuencia)
        {
            if (id != frecuencia.Id)
            {
                return BadRequest();
            }

            var frecuenciaExistente = await _context.Frecuencia.FindAsync(id);
            if (frecuenciaExistente == null)
            {
                return NotFound();
            }

            _context.Entry(frecuenciaExistente).CurrentValues.SetValues(frecuencia);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(frecuencia);
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
            var frecuencia = await _context.Frecuencia.FindAsync(id);
            if (frecuencia == null)
            {
                return NotFound();
            }
            _context.Frecuencia.Remove(frecuencia);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
