using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariablesPorIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public VariablesPorIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.VariablesPorIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(VariablesPorIndicador variablePorIndicador)
        {
            try
            {
                _context.VariablesPorIndicador.Add(variablePorIndicador);
                await _context.SaveChangesAsync();
                return Ok(variablePorIndicador);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var variablePorIndicador = await _context.VariablesPorIndicador.FindAsync(id);
            if (variablePorIndicador == null)
            {
                return NotFound();
            }
            return Ok(variablePorIndicador);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, VariablesPorIndicador variablePorIndicador)
        {
            if (id != variablePorIndicador.Id)
            {
                return BadRequest();
            }

            var existente = await _context.VariablesPorIndicador.FindAsync(id);
            if (existente == null)
            {
                return NotFound();
            }

            _context.Entry(existente).CurrentValues.SetValues(variablePorIndicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(variablePorIndicador);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var variablePorIndicador = await _context.VariablesPorIndicador.FindAsync(id);
            if (variablePorIndicador == null)
            {
                return NotFound();
            }
            _context.VariablesPorIndicador.Remove(variablePorIndicador);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
