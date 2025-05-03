using CRUD.Backend.Data;
using CRUD.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var usuario = await _context.Usuario
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.Contrasena == request.Contrasena);

        if (usuario == null)
        {
            return Unauthorized(new LoginResponse { Exito = false, Mensaje = "Credenciales incorrectas" });
        }

        var roles = await _context.RolUsuario
            .Where(ru => ru.FkEmail == request.Email)
            .Join(_context.Rol,
                  ru => ru.FkIdRol,
                  r => r.Id,
                  (ru, r) => r.Nombre)
            .ToListAsync();

        // Definir jerarquía de roles
        var jerarquia = new List<string> { "Administrador", "Validador", "Verificador" };

        // Buscar el rol más alto según jerarquía
        var rolPrincipal = jerarquia.FirstOrDefault(r => roles.Contains(r)) ?? roles.FirstOrDefault() ?? "";

        return Ok(new LoginResponse
        {
            Exito = true,
            Mensaje = "Login exitoso",
            Email = usuario.Email,
            Roles = roles,
            RolPrincipal = rolPrincipal
        });
    }
}