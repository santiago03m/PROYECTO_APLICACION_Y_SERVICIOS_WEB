using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariableController : ControllerBase
    {
        private readonly DataContext _context;

        public VariableController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Variable.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Variable variable)
        {
            try
            {
                _context.Variable.Add(variable);
                await _context.SaveChangesAsync();
                return Ok(variable);
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
            var variable = await _context.Variable.FindAsync(id);
            if (variable == null)
            {
                return NotFound();
            }
            return Ok(variable);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Variable variable)
        {
            if (id != variable.Id)
            {
                return BadRequest();
            }

            var variableExistente = await _context.Variable.FindAsync(id);
            if (variableExistente == null)
            {
                return NotFound();
            }

            _context.Entry(variableExistente).CurrentValues.SetValues(variable);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(variable);
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
            var variable = await _context.Variable.FindAsync(id);
            if (variable == null)
            {
                return NotFound();
            }
            _context.Variable.Remove(variable);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
