using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;

        public UsuariosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Usuario.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Usuario usuario)
        {
            try
            { _context.Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                return Ok(usuario);
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

        [HttpGet("{email}")]
        public async Task<ActionResult> GetAsync(string email)
        {
            var usuario = await _context.Usuario.FindAsync(email);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<Usuario>> PutAsync(string email, [FromBody] Usuario usuario)
        {
            if (usuario == null)
            {
                return BadRequest("El objeto Usuario es nulo.");
            }

            if (email != usuario.Email)
            {
                return BadRequest("El email del parámetro no coincide con el del objeto.");
            }

            var usuarioExistente = await _context.Usuario.FindAsync(email);
            if (usuarioExistente == null)
            {
                return NotFound($"No se encontró un usuario con el email: {email}");
            }

            // Actualiza solo los valores modificados en la entidad existente
            _context.Entry(usuarioExistente).CurrentValues.SetValues(usuario);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(usuario);
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


        [HttpDelete("{email}")]
        public async Task<ActionResult> DeleteAsync(string email)
        {
            var usuario = await _context.Usuario.FindAsync(email);
            if (usuario == null)
            {
                return NotFound();
            }
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
