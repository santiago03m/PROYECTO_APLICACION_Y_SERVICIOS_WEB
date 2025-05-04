using System.Data.Common;
using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
            try
            {
                var fuentes = await _context.FuentesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerTodasLasFuentesPorIndicador")
                    .ToListAsync();

                return Ok(fuentes);
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
        public async Task<ActionResult> PostAsync(FuentePorIndicador fpi)
        {
            try
            {
                var fkidfuenteParam = new SqlParameter("@fkidfuente", fpi.FkIdFuente);
                var fkidindicadorParam = new SqlParameter("@fkidindicador", fpi.FkIdIndicador);

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertarFuentePorIndicador @fkidfuente, @fkidindicador", fkidfuenteParam, fkidindicadorParam);

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
            try
            {
                var resultados = await _context.FuentesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerFuentePorIndicador @fkidfuente = {0}, @fkidindicador = {1}", fkidfuente, fkidindicador)
                    .AsNoTracking()
                    .ToListAsync();

                var fuente = resultados.FirstOrDefault();

                if (fuente == null)
                    return NotFound();

                return Ok(fuente);
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

        [HttpPut("{fkidfuente}/{fkidindicador}")]
        public Task<ActionResult> PutAsync(int fkidfuente, int fkidindicador, FuentePorIndicador fuentePorIndicador)
        {
            // No se requiere actualizar esta tabla intermedia
            return Task.FromResult<ActionResult>(
                BadRequest("Esta tabla no permite actualización, solo inserción y eliminación.")
            );
        }

        [HttpDelete("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> DeleteAsync(int fkidfuente, int fkidindicador)
        {
            try
            {
                var fkidfuenteParam = new SqlParameter("@fkidfuente", fkidfuente);
                var fkidindicadorParam = new SqlParameter("@fkidindicador", fkidindicador);

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EliminarFuentePorIndicador @fkidfuente, @fkidindicador",
                    fkidfuenteParam, fkidindicadorParam);

                return Ok("Fuente eliminada exitosamente.");
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
