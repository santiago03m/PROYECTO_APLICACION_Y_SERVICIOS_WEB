using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly DataContext _context;

        public ActorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Actor.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Actor actor)
        {
            try
            {
                _context.Actor.Add(actor);
                await _context.SaveChangesAsync();
                return Ok(actor);
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
            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(actor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            var actorExistente = await _context.Actor.FindAsync(id);
            if (actorExistente == null)
            {
                return NotFound();
            }

            _context.Entry(actorExistente).CurrentValues.SetValues(actor);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(actor);
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
            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            _context.Actor.Remove(actor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
