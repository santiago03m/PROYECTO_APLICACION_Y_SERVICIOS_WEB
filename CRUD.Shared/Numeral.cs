using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class Numeral
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FkIdLiteral { get; set; } = string.Empty;
    }
}
