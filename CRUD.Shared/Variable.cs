using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class Variable
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string FkEmailUsuario { get; set; } = string.Empty;
    }
}