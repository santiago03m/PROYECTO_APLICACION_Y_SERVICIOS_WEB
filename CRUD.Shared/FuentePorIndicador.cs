using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class FuentePorIndicador
    {
        [Key]
        public int FkIdFuente { get; set; }

        [Key]
        public int FkIdIndicador { get; set; }
    }
}
