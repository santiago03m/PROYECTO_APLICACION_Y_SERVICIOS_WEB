using CRUD.Backend.Data;
using CRUD.Shared;
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
            try
            {
                var lista = await _context.ResponsablesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerResponsablesPorIndicador")
                    .ToListAsync();  
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{fkidresponsable}/{fkidindicador}")]
        public async Task<ActionResult> GetAsync(string fkidresponsable, int fkidindicador)
        {
            try
            {
                var resultados = await _context.ResponsablesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerResponsablePorIndicador @p0, @p1", fkidresponsable, fkidindicador)
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

        [HttpPost]
        public async Task<ActionResult> PostAsync(ResponsablesPorIndicador rpi)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarResponsablePorIndicador @p0, @p1",
                    rpi.FkIdResponsable, rpi.FkIdIndicador
                );
                return Ok(rpi);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{fkidresponsable}/{fkidindicador}")]
        public async Task<ActionResult> DeleteAsync(string fkidresponsable, int fkidindicador)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EliminarResponsablePorIndicador @p0, @p1",
                    fkidresponsable, fkidindicador
                );
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{fkidresponsable}/{fkidindicador}")]
        public Task<ActionResult> PutAsync(string fkidresponsable, int fkidindicador, ResponsablesPorIndicador rpi)
        {
            // No se requiere actualizar esta tabla intermedia
            return Task.FromResult<ActionResult>(
                BadRequest("Esta tabla no permite actualización, solo inserción y eliminación.")
            );
        }
    }
}
