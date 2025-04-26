using System;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Shared
{
    public class ResultadoIndicador
    {
        [Key]
        public int Id { get; set; }
        
        public float Resultado { get; set; }
        
        public DateTime FechaCalculo { get; set; }
        
        public int FkIdIndicador { get; set; }
    }
}
