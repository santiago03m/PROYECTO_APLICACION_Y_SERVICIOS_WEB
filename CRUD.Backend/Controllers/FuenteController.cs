using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuenteController : ControllerBase
    {
        private readonly DataContext _context;

        public FuenteController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Fuente.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Fuente fuente)
        {
            try
            {
                _context.Fuente.Add(fuente);
                await _context.SaveChangesAsync();
                return Ok(fuente);
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
            var fuente = await _context.Fuente.FindAsync(id);
            if (fuente == null)
            {
                return NotFound();
            }
            return Ok(fuente);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Fuente fuente)
        {
            if (id != fuente.Id)
            {
                return BadRequest();
            }

            var fuenteExistente = await _context.Fuente.FindAsync(id);
            if (fuenteExistente == null)
            {
                return NotFound();
            }

            _context.Entry(fuenteExistente).CurrentValues.SetValues(fuente);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(fuente);
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
            var fuente = await _context.Fuente.FindAsync(id);
            if (fuente == null)
            {
                return NotFound();
            }
            _context.Fuente.Remove(fuente);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
