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
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<FuentePorIndicador> FuentesPorIndicador { get; set; }
        public DbSet<ResultadoIndicador> ResultadoIndicador { get; set; }
        public DbSet<Literal> Literal { get; set; }
        public DbSet<Numeral> Numeral { get; set; }
        public DbSet<Paragrafo> Paragrafo { get; set; }
        public DbSet<Variable> Variable { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<RolUsuario> RolUsuario { get; set; }
        public DbSet<Indicador> Indicador { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FuentePorIndicador>()
                .HasKey(f => new { f.FkIdFuente, f.FkIdIndicador });

            modelBuilder.Entity<RolUsuario>()
                .HasKey(r => new { r.FkEmail, r.FkIdRol });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RolUsuario>()
                .ToTable("rol_usuario");

            modelBuilder.Entity<FuentePorIndicador>()
                .HasKey(f => new { f.FkIdFuente, f.FkIdIndicador });

            modelBuilder.Entity<RolUsuario>()
                .HasKey(r => new { r.FkEmail, r.FkIdRol });
            
            modelBuilder.Entity<RolUsuario>()
                .ToTable("rol_usuario");

            modelBuilder.Entity<Indicador>(entity =>
            {
                entity.HasKey(i => i.Id); 

                entity.HasOne<TipoIndicador>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdTipoIndicador)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne<UnidadMedicion>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdUnidadMedicion)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Sentido>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdSentido)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Frecuencia>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdFrecuencia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Articulo>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdArticulo)
                    .OnDelete(DeleteBehavior.SetNull); 

                entity.HasOne<Literal>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdLiteral)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Numeral>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdNumeral)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne<Paragrafo>()
                    .WithMany()
                    .HasForeignKey(i => i.FkIdParagrafo)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }

    }
}
