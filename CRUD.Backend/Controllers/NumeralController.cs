using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeralController : ControllerBase
    {
        private readonly DataContext _context;

        public NumeralController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Numeral.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Numeral numeral)
        {
            try
            {
                _context.Numeral.Add(numeral);
                await _context.SaveChangesAsync();
                return Ok(numeral);
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
            var numeral = await _context.Numeral.FindAsync(id);
            if (numeral == null)
            {
                return NotFound();
            }
            return Ok(numeral);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Numeral numeral)
        {
            if (id != numeral.Id)
            {
                return BadRequest();
            }

            var numeralExistente = await _context.Numeral.FindAsync(id);
            if (numeralExistente == null)
            {
                return NotFound();
            }

            _context.Entry(numeralExistente).CurrentValues.SetValues(numeral);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(numeral);
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
            var numeral = await _context.Numeral.FindAsync(id);
            if (numeral == null)
            {
                return NotFound();
            }
            _context.Numeral.Remove(numeral);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
