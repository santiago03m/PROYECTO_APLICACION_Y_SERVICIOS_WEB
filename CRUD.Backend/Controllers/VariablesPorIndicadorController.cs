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
    public class VariablesPorIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public VariablesPorIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                // Realiza la consulta asincrónica directamente antes de AsEnumerable()
                var lista = await _context.VariablesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerVariablesPorIndicador")
                    .ToListAsync();  // Realiza la operación asincrónica aquí, antes de AsEnumerable()

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
        public async Task<ActionResult> PostAsync(VariablesPorIndicador variablePorIndicador)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarVariablePorIndicador @p0, @p1, @p2, @p3, @p4",
                    variablePorIndicador.FkIdVariable,
                    variablePorIndicador.FkIdIndicador,
                    variablePorIndicador.Dato,
                    variablePorIndicador.FkEmailUsuario,
                    variablePorIndicador.FechaDato
                );

                return Ok(variablePorIndicador);
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
        public async Task<ActionResult> GetAsync(int id)
        {
            try
            {
                var registros = await _context.VariablesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerVariablePorIndicador @p0", id)
                    .ToListAsync();

                var registro = registros.FirstOrDefault();

                if (registro == null)
                    return NotFound();

                return Ok(registro);
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

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, VariablesPorIndicador variablePorIndicador)
        {
            if (id != variablePorIndicador.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            try
            {
                var parameters = new[]
                {
                new SqlParameter("@id", variablePorIndicador.Id),
                new SqlParameter("@fkidvariable", variablePorIndicador.FkIdVariable),
                new SqlParameter("@fkidindicador", variablePorIndicador.FkIdIndicador),
                new SqlParameter("@dato", variablePorIndicador.Dato),
                new SqlParameter("@fkemailusuario", variablePorIndicador.FkEmailUsuario),
                new SqlParameter("@fechadato", variablePorIndicador.FechaDato)
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_ActualizarVariablePorIndicador @id, @fkidvariable, @fkidindicador, @dato, @fkemailusuario, @fechadato",
                    parameters);

                return Ok("Variable actualizada correctamente.");
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error SQL: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error inesperado: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_EliminarVariablePorIndicador @p0", id
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
