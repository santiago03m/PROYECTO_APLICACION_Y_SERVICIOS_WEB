using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Shared
{
    public class Usuario
    {
        [Key]
        public string Email { get; set; }= string.Empty;
        public string Contrasena { get; set; } = string.Empty;
    }
}
