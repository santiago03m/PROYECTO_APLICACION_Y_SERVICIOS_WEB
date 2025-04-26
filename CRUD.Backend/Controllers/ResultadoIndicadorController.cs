using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultadoIndicadorController : ControllerBase
    {
        private readonly DataContext _context;

        public ResultadoIndicadorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.ResultadoIndicador.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(ResultadoIndicador resultadoIndicador)
        {
            try
            {
                _context.ResultadoIndicador.Add(resultadoIndicador);
                await _context.SaveChangesAsync();
                return Ok(resultadoIndicador);
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
            var resultadoIndicador = await _context.ResultadoIndicador.FindAsync(id);
            if (resultadoIndicador == null)
            {
                return NotFound();
            }
            return Ok(resultadoIndicador);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, ResultadoIndicador resultadoIndicador)
        {
            if (id != resultadoIndicador.Id)
            {
                return BadRequest();
            }

            var resultadoIndicadorExistente = await _context.ResultadoIndicador.FindAsync(id);
            if (resultadoIndicadorExistente == null)
            {
                return NotFound();
            }

            _context.Entry(resultadoIndicadorExistente).CurrentValues.SetValues(resultadoIndicador);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(resultadoIndicador);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var resultadoIndicador = await _context.ResultadoIndicador.FindAsync(id);
            if (resultadoIndicador == null)
            {
                return NotFound();
            }
            _context.ResultadoIndicador.Remove(resultadoIndicador);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
