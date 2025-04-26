using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class VariablesPorIndicador
    {
        [Key]
        public int Id { get; set; }
        public int FkIdVariable { get; set; }
        public int FkIdIndicador { get; set; }
        public double Dato { get; set; }
        public string FkEmailUsuario { get; set; } = string.Empty;
        public DateTime FechaDato { get; set; }
    }
}

