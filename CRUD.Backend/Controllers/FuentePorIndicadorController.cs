using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

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

        // Consultar todos
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Consultar uno
        [HttpGet("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> GetAsync(int fkidfuente, int fkidindicador)
        {
            try
            {
                var resultados = await _context.FuentesPorIndicador
                    .FromSqlRaw("EXEC sp_ObtenerFuentePorIndicador @fkidfuente = {0}, @fkidindicador = {1}", fkidfuente, fkidindicador)
                    .AsNoTracking()
                    .ToListAsync();  // Ejecuta primero

                var fuente = resultados.FirstOrDefault();  // Filtra después

                if (fuente == null)
                    return NotFound();

                return Ok(fuente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // Insertar
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] FuentePorIndicador fpi)
        {
            try
            {
                var fkidfuenteParam = new SqlParameter("@fkidfuente", fpi.FkIdFuente);
                var fkidindicadorParam = new SqlParameter("@fkidindicador", fpi.FkIdIndicador);

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_InsertarFuentePorIndicador @fkidfuente, @fkidindicador", fkidfuenteParam, fkidindicadorParam);

                return Ok(fpi);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Eliminar
        [HttpDelete("{fkidfuente}/{fkidindicador}")]
        public async Task<ActionResult> DeleteAsync(int fkidfuente, int fkidindicador)
        {
            try
            {
                var fkidfuenteParam = new SqlParameter("@fkidfuente", fkidfuente);
                var fkidindicadorParam = new SqlParameter("@fkidindicador", fkidindicador);

                await _context.Database.ExecuteSqlRawAsync("EXEC sp_EliminarFuentePorIndicador @fkidfuente, @fkidindicador", fkidfuenteParam, fkidindicadorParam);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
