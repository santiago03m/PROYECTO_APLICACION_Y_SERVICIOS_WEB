using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly DataContext _context;

        public ArticuloController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Articulo.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(Articulo articulo)
        {
            try
            {
                _context.Articulo.Add(articulo);
                await _context.SaveChangesAsync();
                return Ok(articulo);
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
            var articulo = await _context.Articulo.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            return Ok(articulo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(string id, Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return BadRequest();
            }

            var articuloExistente = await _context.Articulo.FindAsync(id);
            if (articuloExistente == null)
            {
                return NotFound();
            }

            _context.Entry(articuloExistente).CurrentValues.SetValues(articulo);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(articulo);
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
            var articulo = await _context.Articulo.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            _context.Articulo.Remove(articulo);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
