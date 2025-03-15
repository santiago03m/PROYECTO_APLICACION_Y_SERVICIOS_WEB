using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentacionVisualController : ControllerBase
    {
        private readonly DataContext _context;

        public RepresentacionVisualController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.RepresenVisual.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(RepresentacionVisual representacionVisual)
        {
            try
            {
                _context.RepresenVisual.Add(representacionVisual);
                await _context.SaveChangesAsync();
                return Ok(representacionVisual);
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
            var representacionVisual = await _context.RepresenVisual.FindAsync(id);
            if (representacionVisual == null)
            {
                return NotFound();
            }
            return Ok(representacionVisual);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, RepresentacionVisual representacionVisual)
        {
            if (id != representacionVisual.Id)
            {
                return BadRequest();
            }

            var representacionVisualExistente = await _context.RepresenVisual.FindAsync(id);
            if (representacionVisualExistente == null)
            {
                return NotFound();
            }

            _context.Entry(representacionVisualExistente).CurrentValues.SetValues(representacionVisual);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(representacionVisual);
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
            var representacionVisual = await _context.RepresenVisual.FindAsync(id);
            if (representacionVisual == null)
            {
                return NotFound();
            }
            _context.RepresenVisual.Remove(representacionVisual);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
