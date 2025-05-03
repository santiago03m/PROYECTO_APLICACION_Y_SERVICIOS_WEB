using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Mvc;
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(VariablesPorIndicador vpi)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_InsertarVariablePorIndicador @p0, @p1, @p2, @p3, @p4",
                    vpi.FkIdVariable,
                    vpi.FkIdIndicador,
                    vpi.Dato,
                    vpi.FkEmailUsuario,
                    vpi.FechaDato
                );

                return Ok(vpi);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, VariablesPorIndicador vpi)
        {
            if (id != vpi.Id)
                return BadRequest("ID no coincide.");

            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC sp_ActualizarVariablePorIndicador @p0, @p1, @p2, @p3, @p4, @p5",
                    vpi.Id,
                    vpi.FkIdVariable,
                    vpi.FkIdIndicador,
                    vpi.Dato,
                    vpi.FkEmailUsuario,
                    vpi.FechaDato
                );

                return Ok(vpi);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
