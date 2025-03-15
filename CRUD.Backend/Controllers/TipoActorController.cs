using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoActorController : ControllerBase
    {
        private readonly DataContext _context;

        public TipoActorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.TipoActor.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(TipoActor tipoActor)
        {
            try
            {
                _context.TipoActor.Add(tipoActor);
                await _context.SaveChangesAsync();
                return Ok(tipoActor);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var tipoActor = await _context.TipoActor.FindAsync(id);
            if (tipoActor == null)
            {
                return NotFound();
            }
            return Ok(tipoActor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, TipoActor tipoActor)
        {
            if (id != tipoActor.Id)
            {
                return BadRequest();
            }

            var tipoActorExistente = await _context.TipoActor.FindAsync(id);
            if (tipoActorExistente == null)
            {
                return NotFound();
            }

            _context.Entry(tipoActorExistente).CurrentValues.SetValues(tipoActor);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(tipoActor);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var tipoActor = await _context.TipoActor.FindAsync(id);
            if (tipoActor == null)
            {
                return NotFound();
            }
            _context.TipoActor.Remove(tipoActor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
