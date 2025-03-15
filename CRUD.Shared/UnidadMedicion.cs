using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared
{
    public class UnidadMedicion
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}
