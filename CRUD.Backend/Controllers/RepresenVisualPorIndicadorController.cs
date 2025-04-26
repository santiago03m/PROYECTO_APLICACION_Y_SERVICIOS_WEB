using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresenVisualPorIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public RepresenVisualPorIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.RepresenVisualPorIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(RepresenVisualPorIndicador represenVisualPorIndicador)
        {
            try
            {
                _context.RepresenVisualPorIndicador.Add(represenVisualPorIndicador);
                await _context.SaveChangesAsync();
                return Ok(represenVisualPorIndicador);
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

        [HttpGet("{fkidindicador}/{fkidrepresenvisual}")]
        public async Task<ActionResult> GetAsync(int fkidindicador, int fkidrepresenvisual)
        {
            var entidad = await _context.RepresenVisualPorIndicador.FindAsync(fkidindicador, fkidrepresenvisual);
            if (entidad == null)
            {
                return NotFound();
            }
            return Ok(entidad);
        }

        [HttpPut("{fkidindicador}/{fkidrepresenvisual}")]
        public async Task<ActionResult> PutAsync(int fkidindicador, int fkidrepresenvisual, RepresenVisualPorIndicador represenVisualPorIndicador)
        {
            if (fkidindicador != represenVisualPorIndicador.FkIdIndicador || fkidrepresenvisual != represenVisualPorIndicador.FkIdRepresenVisual)
            {
                return BadRequest();
            }

            var entidadExistente = await _context.RepresenVisualPorIndicador.FindAsync(fkidindicador, fkidrepresenvisual);
            if (entidadExistente == null)
            {
                return NotFound();
            }

            _context.Entry(entidadExistente).CurrentValues.SetValues(represenVisualPorIndicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(represenVisualPorIndicador);
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

        [HttpDelete("{fkidindicador}/{fkidrepresenvisual}")]
        public async Task<ActionResult> DeleteAsync(int fkidindicador, int fkidrepresenvisual)
        {
            var entidad = await _context.RepresenVisualPorIndicador.FindAsync(fkidindicador, fkidrepresenvisual);
            if (entidad == null)
            {
                return NotFound();
            }
            _context.RepresenVisualPorIndicador.Remove(entidad);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
