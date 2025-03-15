using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared
{
    public class Seccion
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }
}
