using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsablesPorIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public ResponsablesPorIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.ResponsablesPorIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ResponsablesPorIndicador ResponsablesPorIndicador)
        {
            try
            {
                _context.ResponsablesPorIndicador.Add(ResponsablesPorIndicador);
                await _context.SaveChangesAsync();
                return Ok(ResponsablesPorIndicador);
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

        [HttpGet("{fkidresponsable}/{fkidindicador}")]
        public async Task<ActionResult> GetAsync(string fkidresponsable, int fkidindicador)
        {
            var ResponsablesPorIndicador = await _context.ResponsablesPorIndicador
                .FindAsync(fkidresponsable, fkidindicador);
            if (ResponsablesPorIndicador == null)
            {
                return NotFound();
            }
            return Ok(ResponsablesPorIndicador);
        }

        [HttpPut("{fkidresponsable}/{fkidindicador}")]
        public async Task<ActionResult> PutAsync(string fkidresponsable, int fkidindicador, ResponsablesPorIndicador ResponsablesPorIndicador)
        {
            if (fkidresponsable != ResponsablesPorIndicador.FkIdResponsable || fkidindicador != ResponsablesPorIndicador.FkIdIndicador)
            {
                return BadRequest();
            }

            var existente = await _context.ResponsablesPorIndicador.FindAsync(fkidresponsable, fkidindicador);
            if (existente == null)
            {
                return NotFound();
            }

            _context.Entry(existente).CurrentValues.SetValues(ResponsablesPorIndicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(ResponsablesPorIndicador);
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

        [HttpDelete("{fkidresponsable}/{fkidindicador}")]
        public async Task<ActionResult> DeleteAsync(string fkidresponsable, int fkidindicador)
        {
            var ResponsablesPorIndicador = await _context.ResponsablesPorIndicador.FindAsync(fkidresponsable, fkidindicador);
            if (ResponsablesPorIndicador == null)
            {
                return NotFound();
            }
            _context.ResponsablesPorIndicador.Remove(ResponsablesPorIndicador);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
