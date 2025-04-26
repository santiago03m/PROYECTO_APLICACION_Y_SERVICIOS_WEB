using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolUsuarioController : ControllerBase
    {
        private readonly DataContext _context;

        public RolUsuarioController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.RolUsuario.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(RolUsuario rolUsuario)
        {
            try
            {
                _context.RolUsuario.Add(rolUsuario);
                await _context.SaveChangesAsync();
                return Ok(rolUsuario);
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

        [HttpGet("{email}/{idRol}")]
        public async Task<ActionResult> GetAsync(string email, int idRol)
        {
            var rolUsuario = await _context.RolUsuario.FindAsync(email, idRol);
            if (rolUsuario == null)
            {
                return NotFound();
            }
            return Ok(rolUsuario);
        }

        [HttpPut("{email}/{idRol}")]
        public async Task<ActionResult> PutAsync(string email, int idRol, RolUsuario rolUsuario)
        {
            if (email != rolUsuario.FkEmail || idRol != rolUsuario.FkIdRol)
            {
                return BadRequest();
            }

            var existente = await _context.RolUsuario.FindAsync(email, idRol);
            if (existente == null)
            {
                return NotFound();
            }

            _context.Entry(existente).CurrentValues.SetValues(rolUsuario);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(rolUsuario);
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

        [HttpDelete("{email}/{idRol}")]
        public async Task<ActionResult> DeleteAsync(string email, int idRol)
        {
            var rolUsuario = await _context.RolUsuario.FindAsync(email, idRol);
            if (rolUsuario == null)
            {
                return NotFound();
            }
            _context.RolUsuario.Remove(rolUsuario);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
