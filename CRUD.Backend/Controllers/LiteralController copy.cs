using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteralController : ControllerBase
    {
        private readonly DataContext _context;

        public LiteralController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Literal.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Literal literal)
        {
            try
            {
                _context.Literal.Add(literal);
                await _context.SaveChangesAsync();
                return Ok(literal);
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
            var literal = await _context.Literal.FindAsync(id);
            if (literal == null)
            {
                return NotFound();
            }
            return Ok(literal);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Literal literal)
        {
            if (id != literal.Id)
            {
                return BadRequest();
            }

            var literalExistente = await _context.Literal.FindAsync(id);
            if (literalExistente == null)
            {
                return NotFound();
            }

            _context.Entry(literalExistente).CurrentValues.SetValues(literal);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(literal);
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
            var literal = await _context.Literal.FindAsync(id);
            if (literal == null)
            {
                return NotFound();
            }
            _context.Literal.Remove(literal);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
