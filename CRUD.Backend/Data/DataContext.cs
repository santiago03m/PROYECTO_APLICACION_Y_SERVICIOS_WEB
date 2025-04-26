using CRUD.Shared;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<TipoIndicador> TipoIndicador { get; set; }
        public DbSet<UnidadMedicion> UnidadMedicion { get; set; }
        public DbSet<TipoActor> TipoActor { get; set; }
        public DbSet<SubSeccion> Subseccion { get; set; }
        public DbSet<Frecuencia> Frecuencia { get; set; }
        public DbSet<Sentido> Sentido { get; set; }
        public DbSet<Seccion> Seccion { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Fuente> Fuente { get; set; }
        public DbSet<RepresentacionVisual> RepresenVisual { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Literal> Literal { get; set; }
        public DbSet<Numeral> Numeral { get; set; }
        public DbSet<Paragrafo> Paragrafo { get; set; }   
        public DbSet<Variable> Variable { get; set; }     
    }
}
