using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared
{
    public class Literal
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FkIdArticulo { get; set; } = string.Empty;
    }
}