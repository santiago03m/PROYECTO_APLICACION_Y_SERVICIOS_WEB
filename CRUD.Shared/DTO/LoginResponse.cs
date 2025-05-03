namespace CRUD.Shared.DTO
{
    public class LoginResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string RolPrincipal { get; set; } = string.Empty;
    }
}
