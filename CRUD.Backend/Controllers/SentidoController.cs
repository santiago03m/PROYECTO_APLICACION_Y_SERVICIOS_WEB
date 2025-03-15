using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SentidoController : ControllerBase
    {
        private readonly DataContext _context;

        public SentidoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Sentido.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Sentido sentido)
        {
            try
            {
                _context.Sentido.Add(sentido);
                await _context.SaveChangesAsync();
                return Ok(sentido);
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
            var sentido = await _context.Sentido.FindAsync(id);
            if (sentido == null)
            {
                return NotFound();
            }
            return Ok(sentido);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Sentido sentido)
        {
            if (id != sentido.Id)
            {
                return BadRequest();
            }

            var sentidoExistente = await _context.Sentido.FindAsync(id);
            if (sentidoExistente == null)
            {
                return NotFound();
            }

            _context.Entry(sentidoExistente).CurrentValues.SetValues(sentido);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(sentido);
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
            var sentido = await _context.Sentido.FindAsync(id);
            if (sentido == null)
            {
                return NotFound();
            }
            _context.Sentido.Remove(sentido);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
