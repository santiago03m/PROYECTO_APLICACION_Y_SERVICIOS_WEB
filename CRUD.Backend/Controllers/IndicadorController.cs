using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public IndicadorController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Indicador
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var indicadores = await _context.Indicador.ToListAsync();
            return Ok(indicadores);
        }

        // GET: api/Indicador/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var indicador = await _context.Indicador.FindAsync(id);
            if (indicador == null)
            {
                return NotFound();
            }
            return Ok(indicador);
        }

        // POST: api/Indicador
        [HttpPost]
        public async Task<ActionResult> PostAsync(Indicador indicador)
        {
            try
            {
                _context.Indicador.Add(indicador);
                await _context.SaveChangesAsync();
                return Ok(indicador);
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

        // PUT: api/Indicador/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Indicador indicador)
        {
            if (id != indicador.Id)
            {
                return BadRequest();
            }

            var indicadorExistente = await _context.Indicador.FindAsync(id);
            if (indicadorExistente == null)
            {
                return NotFound();
            }

            _context.Entry(indicadorExistente).CurrentValues.SetValues(indicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(indicador);
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

        // DELETE: api/Indicador/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var indicador = await _context.Indicador.FindAsync(id);
            if (indicador == null)
            {
                return NotFound();
            }
            _context.Indicador.Remove(indicador);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
