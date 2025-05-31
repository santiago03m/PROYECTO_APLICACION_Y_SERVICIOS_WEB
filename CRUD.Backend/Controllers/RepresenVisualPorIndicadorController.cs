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
            try
            {
                var lista = await _context.RepresenVisualPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerRepresenVisualPorIndicador")
                    .ToListAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // CAMBIA ESTE MÉTODO:
        [HttpPost]
        public async Task<ActionResult> PostAsync(RepresenVisualPorIndicador represenVisualPorIndicador)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarRepresenVisualPorIndicador @p0, @p1",
                    represenVisualPorIndicador.FkIdIndicador,
                    represenVisualPorIndicador.FkIdRepresenVisual
                );
                return Ok(represenVisualPorIndicador);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // CAMBIA ESTE MÉTODO:
        [HttpGet("{fkidindicador}/{fkidrepresenvisual}")]
        public async Task<ActionResult> GetAsync(int fkidindicador, int fkidrepresenvisual)
        {
            try
            {
                var resultados = await _context.RepresenVisualPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerRepresenVisualPorIndicadorUno @p0, @p1", fkidindicador, fkidrepresenvisual)
                    .AsNoTracking()
                    .ToListAsync();

                var resultado = resultados.FirstOrDefault();

                if (resultado == null)
                    return NotFound();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{fkidindicador}/{fkidrepresenvisual}")]
        public Task<ActionResult> PutAsync(int fkidindicador, int fkidrepresenvisual, RepresenVisualPorIndicador represenVisualPorIndicador)
        {
            // No se requiere actualizar esta tabla intermedia
            return Task.FromResult<ActionResult>(
                BadRequest("Esta tabla no permite actualización, solo inserción y eliminación.")
            );
        }

        [HttpDelete("{fkidindicador}/{fkidrepresenvisual}")]
        public async Task<ActionResult> DeleteAsync(int fkidindicador, int fkidrepresenvisual)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EliminarRepresenVisualPorIndicador @p0, @p1",
                    fkidindicador, fkidrepresenvisual
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
