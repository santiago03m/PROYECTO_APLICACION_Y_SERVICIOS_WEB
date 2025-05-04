using System.Data.Common;
using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            catch (SqlException ex)
            {
                return StatusCode(500, $"Error de base de datos: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, $"Operación inválida: {ex.Message}");
            }
            catch (DbException ex)
            {
                return StatusCode(500, $"Error de acceso a datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ResponsablesPorIndicador ResponsablesPorIndicador)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarResponsablePorIndicador @p0, @p1",
                    ResponsablesPorIndicador.FkIdResponsable, ResponsablesPorIndicador.FkIdIndicador
                );
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
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error en la base de datos: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error en la operación: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error inesperado: {ex.Message}");
            }
        }

        [HttpPut("{fkidresponsable}/{fkidindicador}")]
        public Task<ActionResult> PutAsync(string fkidresponsable, int fkidindicador, ResponsablesPorIndicador ResponsablesPorIndicador)
        {
            // No se requiere actualizar esta tabla intermedia
            return Task.FromResult<ActionResult>(
                BadRequest("Esta tabla no permite actualización, solo inserción y eliminación.")
            );
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
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error en la base de datos: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al eliminar los datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
