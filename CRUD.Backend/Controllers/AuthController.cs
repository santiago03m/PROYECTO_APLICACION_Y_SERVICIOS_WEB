using CRUD.Backend.Data;
using CRUD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace CRUD.Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly string secretKey;
        private readonly IConfiguration _configuration;
        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            secretKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Password, usuario.Contrasena))
            {
                return BadRequest(new { Message = "Credenciales inválidas", isSuccess = false });
            }

            var rolesIds = await _context.RolUsuario
                .Where(ru => ru.FkEmail == usuario.Email)
                .Select(ru => ru.FkIdRol)
                .ToListAsync();

            var nombresRoles = await _context.Rol
                .Where(r => rolesIds.Contains(r.Id))
                .Select(r => r.Nombre)
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
            };
            claims.AddRange(nombresRoles.Select(rol => new Claim(ClaimTypes.Role, rol)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMonths(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Message = "Inicio de sesión exitoso.",
                isSuccess = true,
                token = tokenString,
                roles = nombresRoles
            });
        }


    }
}