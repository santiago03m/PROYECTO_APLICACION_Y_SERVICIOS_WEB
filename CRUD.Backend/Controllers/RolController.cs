using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly DataContext _context;

        public RolController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Rol.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Rol rol)
        {
            try
            {
                _context.Rol.Add(rol);
                await _context.SaveChangesAsync();
                return Ok(rol);
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
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }

            var rolExistente = await _context.Rol.FindAsync(id);
            if (rolExistente == null)
            {
                return NotFound();
            }

            _context.Entry(rolExistente).CurrentValues.SetValues(rol);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(rol);
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
            var rol = await _context.Rol.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            _context.Rol.Remove(rol);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
