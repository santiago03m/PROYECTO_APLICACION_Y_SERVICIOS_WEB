using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class Paragrafo
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FkIdArticulo { get; set; } = string.Empty;
    }
}