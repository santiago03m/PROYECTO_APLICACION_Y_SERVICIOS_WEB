using System.ComponentModel.DataAnnotations;


namespace CRUD.Shared
{
    public class Articulo
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FkIdSeccion { get; set; } = string.Empty;
        public string FkIdSubseccion { get; set; } = string.Empty;
    }
}
