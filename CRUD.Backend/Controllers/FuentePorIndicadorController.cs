using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuentePorIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public FuentePorIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.FuentesPorIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(FuentePorIndicador fpi)
        {
            try
            {
                _context.FuentesPorIndicador.Add(fpi);
                await _context.SaveChangesAsync();
                return Ok(fpi);
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

        [HttpGet("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> GetAsync(int fkidfuente, int fkidindicador)
        {
            var fpi = await _context.FuentesPorIndicador.FindAsync(fkidfuente, fkidindicador);
            if (fpi == null)
            {
                return NotFound();
            }
            return Ok(fpi);
        }

        [HttpPut("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> PutAsync(int fkidfuente, int fkidindicador, FuentePorIndicador fpi)
        {
            if (fkidfuente != fpi.FkIdFuente || fkidindicador != fpi.FkIdIndicador)
            {
                return BadRequest();
            }

            var fpiExistente = await _context.FuentesPorIndicador.FindAsync(fkidfuente, fkidindicador);
            if (fpiExistente == null)
            {
                return NotFound();
            }

            _context.Entry(fpiExistente).CurrentValues.SetValues(fpi);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(fpi);
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

        [HttpDelete("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> DeleteAsync(int fkidfuente, int fkidindicador)
        {
            var fpi = await _context.FuentesPorIndicador.FindAsync(fkidfuente, fkidindicador);
            if (fpi == null)
            {
                return NotFound();
            }
            _context.FuentesPorIndicador.Remove(fpi);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
