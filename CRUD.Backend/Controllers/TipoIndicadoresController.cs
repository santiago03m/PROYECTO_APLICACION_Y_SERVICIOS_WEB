using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoIndicadoresController : ControllerBase
    {
        private readonly DataContext _context;

        public TipoIndicadoresController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.TipoIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(TipoIndicador tipoIndicador)
        {
            try
            {
                _context.TipoIndicador.Add(tipoIndicador);
                await _context.SaveChangesAsync();
                return Ok(tipoIndicador);
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
            var tipoIndicador = await _context.TipoIndicador.FindAsync(id);
            if (tipoIndicador == null)
            {
                return NotFound();
            }
            return Ok(tipoIndicador);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, TipoIndicador tipoIndicador)
        {
            if (id != tipoIndicador.Id)
            {
                return BadRequest();
            }

            var tipoIndicadorExistente = await _context.TipoIndicador.FindAsync(id);
            if (tipoIndicadorExistente == null)
            {
                return NotFound();
            }

            _context.Entry(tipoIndicadorExistente).CurrentValues.SetValues(tipoIndicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(tipoIndicador);
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
            var tipoIndicador = await _context.TipoIndicador.FindAsync(id);
            if (tipoIndicador == null)
            {
                return NotFound();
            }
            _context.TipoIndicador.Remove(tipoIndicador);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}