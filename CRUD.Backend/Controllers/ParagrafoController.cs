using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParagrafoController : ControllerBase
    {
        private readonly DataContext _context;

        public ParagrafoController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Paragrafo.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Paragrafo paragrafo)
        {
            try
            {
                _context.Paragrafo.Add(paragrafo);
                await _context.SaveChangesAsync();
                return Ok(paragrafo);
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
            var paragrafo = await _context.Paragrafo.FindAsync(id);
            if (paragrafo == null)
            {
                return NotFound();
            }
            return Ok(paragrafo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Paragrafo paragrafo)
        {
            if (id != paragrafo.Id)
            {
                return BadRequest();
            }

            var paragrafoExistente = await _context.Paragrafo.FindAsync(id);
            if (paragrafoExistente == null)
            {
                return NotFound();
            }

            _context.Entry(paragrafoExistente).CurrentValues.SetValues(paragrafo);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(paragrafo);
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
            var paragrafo = await _context.Paragrafo.FindAsync(id);
            if (paragrafo == null)
            {
                return NotFound();
            }
            _context.Paragrafo.Remove(paragrafo);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}